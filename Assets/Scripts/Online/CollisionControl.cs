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
                // 플레이어의 발이 대상의 y좌표보다 높을 시 타격으로 인정
                if (rigid2D.position.y - 0.5f * height > collision.transform.position.y)
                {
                    var enemyPhotonView = collision.gameObject.GetComponent<PhotonView>();
                    if (enemyPhotonView != null)
                        enemyPhotonView.RPC("DestroyEnemy", RpcTarget.MasterClient);
                    //이펙트 생성 생성된 이펙트는 자동적으로 destroy됨 (DeathEffect.cs 참고)
                    //Instantiate(Death, new Vector3(collision.transform.position.x, collision.transform.position.y, 0), Death.transform.rotation);
                    //공격할 경우에는 점프를 시켜준다.
                    playerMove.state = PlayerMove.State.jump;
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

    void OnDamaged(Vector2 targetPos)
    {
        GameManager.instance.PlayerHP -= 1;
        playerMove.state = PlayerMove.State.hurt;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid2D.velocity = new Vector2(dirc, 1) * 3;
        Invoke("HurtControl", 0.5f);
        //무적시간은 2초
        Invoke("OffDamaged", 1);
    }

    void HurtControl()
    {
        //hurt가 끝나면 상태를 다시 idle로, 버그를 막기 위해서 점프카운트를 초기화 시켜준다 
        playerMove.state = PlayerMove.State.idle;
    }

    void OffDamaged()
    {
        //무적 해제
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
