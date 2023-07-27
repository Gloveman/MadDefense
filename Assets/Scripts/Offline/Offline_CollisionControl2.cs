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
        //�ǰݻ��� ����
        if (collision.gameObject.tag == "Enemy")
        {
            // �÷��̾��� ���� ����� y��ǥ���� ���� �� Ÿ������ ����
            if (rigid2D.position.y - 0.5f * height > collision.transform.position.y)
            {
                GetComponent<AudioSource>().PlayOneShot(hiteffect);
                collision.gameObject.SetActive(false);
                //����Ʈ ���� ������ ����Ʈ�� �ڵ������� destroy�� (DeathEffect.cs ����)
                //Instantiate(Death, new Vector3(collision.transform.position.x, collision.transform.position.y, 0), Death.transform.rotation);
                //������ ��쿡�� ������ �����ش�.
                offline_Player2.state = Offline_Player2.State.jump;
                offline_Player2.Jump();
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
        //�����ð��� 2��
        Invoke("OffDamaged", 1);
    }

    void HurtControl()
    {
        //hurt�� ������ ���¸� �ٽ� idle��, ���׸� ���� ���ؼ� ����ī��Ʈ�� �ʱ�ȭ �����ش� 
        offline_Player2.state = Offline_Player2.State.idle;
    }

    void OffDamaged()
    {
        //���� ����
        gameObject.layer = 0;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
