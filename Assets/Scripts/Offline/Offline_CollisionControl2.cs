using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Offline_CollisionControl2 : MonoBehaviour
{
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    Offline_Player2 offline_Player2;

    [SerializeField]
    AudioClip hiteffect;
    [SerializeField]
    AudioClip hurteffect;

    public float height = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        offline_Player2 = gameObject.GetComponent<Offline_Player2>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        //피격상태 설정
        if (collision.gameObject.tag == "Enemy")
        {
            // 플레이어의 발이 대상의 y좌표보다 높을 시 타격으로 인정
            if (rigid2D.position.y - 0.5f * height > collision.transform.position.y)
            {
                GetComponent<AudioSource>().PlayOneShot(hiteffect);
                collision.gameObject.SetActive(false);
                //이펙트 생성 생성된 이펙트는 자동적으로 destroy됨 (DeathEffect.cs 참고)
                //Instantiate(Death, new Vector3(collision.transform.position.x, collision.transform.position.y, 0), Death.transform.rotation);
                //공격할 경우에는 점프를 시켜준다.
                offline_Player2.state = Offline_Player2.State.jump;
                offline_Player2.Jump();
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

    public void OnDamaged(Vector2 targetPos)
    {
        GetComponent<AudioSource>().PlayOneShot(hurteffect);
        TutorialGameManager.instance.PlayerHP -= 1;
        offline_Player2.state = Offline_Player2.State.hurt;

        gameObject.layer = 10;
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
        offline_Player2.state = Offline_Player2.State.idle;
    }

    void OffDamaged()
    {
        //무적 해제
        gameObject.layer = 0;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
