using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CollisionControl : MonoBehaviourPun
{
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    PlayerMove playerMove;
    public float height = 0.5f;
    Queue<GameObject> collisionList;
    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMove = gameObject.GetComponent<PlayerMove>();
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
                        enemyPhotonView.RPC("DestroyEnemy", RpcTarget.MasterClient);
                    //����Ʈ ���� ������ ����Ʈ�� �ڵ������� destroy�� (DeathEffect.cs ����)
                    //Instantiate(Death, new Vector3(collision.transform.position.x, collision.transform.position.y, 0), Death.transform.rotation);
                    //������ ��쿡�� ������ �����ش�.
                    playerMove.state = PlayerMove.State.jump;
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

    void OnDamaged(Vector2 targetPos)
    {
        GameManager.instance.PlayerHP -= 1;
        playerMove.state = PlayerMove.State.hurt;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid2D.velocity = new Vector2(dirc, 1) * 3;
        Invoke("HurtControl", 0.5f);
        //�����ð��� 2��
        Invoke("OffDamaged", 1);
    }

    void HurtControl()
    {
        //hurt�� ������ ���¸� �ٽ� idle��, ���׸� ���� ���ؼ� ����ī��Ʈ�� �ʱ�ȭ �����ش� 
        playerMove.state = PlayerMove.State.idle;
    }

    void OffDamaged()
    {
        //���� ����
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
