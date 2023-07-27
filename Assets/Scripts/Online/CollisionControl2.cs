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
                // 플레이어의 발이 대상의 y좌표보다 높을 시 타격으로 인정
                if (rigid2D.position.y - 0.5f * height > collision.transform.position.y)
                {
                    var enemyPhotonView = collision.gameObject.GetComponent<PhotonView>();
                    if (enemyPhotonView != null)
                    {
                        photonView.RPC("DestroySound", RpcTarget.All);
                        enemyPhotonView.RPC("DestroyEnemy", RpcTarget.MasterClient);
                        PhotonNetwork.Instantiate("EnemyDead", collision.gameObject.transform.position, new Quaternion());
                    }
                    //이펙트 생성 생성된 이펙트는 자동적으로 destroy됨 (DeathEffect.cs 참고)
                    //Instantiate(Death, new Vector3(collision.transform.position.x, collision.transform.position.y, 0), Death.transform.rotation);
                    //공격할 경우에는 점프를 시켜준다.
                    player2.state = Player2.State.jump;
                    player2.Jump();
                }
                //타격이 아닌 닿음일 경우 피격
                else
                    OnDamaged(collision.transform.position);
            }
            else if (collision.gameObject.tag == "Spike")
            {
                OnDamaged(collision.transform.position);
            }
        }
        //피격상태 설정
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
        //무적시간은 2초
        Invoke("OffDamaged", 1.5f);
    }

    void HurtControl()
    {
        //hurt가 끝나면 상태를 다시 idle로, 버그를 막기 위해서 점프카운트를 초기화 시켜준다 
        player2.state = Player2.State.idle;
    }

    void OffDamaged()
    {
        //무적 해제

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
