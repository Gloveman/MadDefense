using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CollisionControl2 : MonoBehaviourPun
{
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    Player2 player2;
    public float height = 0.5f;
    Queue<GameObject> collisionList;

    [SerializeField]
    AudioClip hurtsound;

    [SerializeField]
    AudioClip Destroysound;

    // Start is called before the first frame update
    public GameObject EnemyDead;

    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        player2 = gameObject.GetComponent<Player2>();
        collisionList = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (photonView.IsMine)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                // �÷��̾��� ���� ����� y��ǥ���� ���� �� Ÿ������ ����
                if (rigid2D.position.y - 0.5f * height > collision.transform.position.y)
                {
                    var enemyPhotonView = collision.gameObject.GetComponent<PhotonView>();
                    if (enemyPhotonView != null)
                    {
                        photonView.RPC("DestroySound", RpcTarget.All);
                        enemyPhotonView.RPC("DestroyEnemy", RpcTarget.MasterClient);
                        PhotonNetwork.Instantiate("EnemyDead", collision.gameObject.transform.position, new Quaternion());
                    }
                    //����Ʈ ���� ������ ����Ʈ�� �ڵ������� destroy�� (DeathEffect.cs ����)
                    //Instantiate(Death, new Vector3(collision.transform.position.x, collision.transform.position.y, 0), Death.transform.rotation);
                    //������ ��쿡�� ������ �����ش�.
                    player2.state = Player2.State.jump;
                    player2.Jump();
                }
                //Ÿ���� �ƴ� ������ ��� �ǰ�
                else
                    OnDamaged(collision.transform.position);
            }
            else if (collision.gameObject.tag == "Spike")
            {
                OnDamaged(collision.transform.position);
            }
        }
        //�ǰݻ��� ����
    }

    public void OnDamaged(Vector2 targetPos)
    {
        photonView.RPC("HurtSound", RpcTarget.All);
        GameManager.instance.PlayerHP -= 1;
        player2.state = Player2.State.hurt;

        if (photonView.IsMine)
            photonView.RPC("ChangeLayer", RpcTarget.All, 10);
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        if (photonView.IsMine)
            photonView.RPC("ChangeSpriteColor", RpcTarget.All, 0.4f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid2D.velocity = new Vector2(dirc, 1) * 3;
        Invoke("HurtControl", 0.5f);
        //�����ð��� 2��
        Invoke("OffDamaged", 1.5f);
    }

    void HurtControl()
    {
        //hurt�� ������ ���¸� �ٽ� idle��, ���׸� ���� ���ؼ� ����ī��Ʈ�� �ʱ�ȭ �����ش� 
        player2.state = Player2.State.idle;
    }

    void OffDamaged()
    {
        //���� ����

        if (photonView.IsMine)
            photonView.RPC("ChangeLayer", RpcTarget.All, 0);


        spriteRenderer.color = new Color(1, 1, 1, 1);
        if (photonView.IsMine)
            photonView.RPC("ChangeSpriteColor", RpcTarget.All, 1f);
    }

    [PunRPC]
    void DestroySound()
    {
        GetComponent<AudioSource>().PlayOneShot(Destroysound);
    }

    [PunRPC]
    void HurtSound()
    {
        GetComponent<AudioSource>().PlayOneShot(hurtsound);
    }
    [PunRPC]
    public void ChangeLayer(int layer)
    {
        gameObject.layer = layer;
    }
    [PunRPC]
    public void ChangeSpriteColor(float alpha)
    {
        spriteRenderer.color = new Color(1, 1, 1, alpha);
    }
}
