using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offline_FrogMove : MonoBehaviour
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float jumpforce = 5.0f;
    Vector3 initpos;
    Rigidbody2D rd;
    GameObject[] player;
    float Chasingcoeff;
    float timer=0f;

    enum State {idle, jump,fall }
    State state=State.idle;
    // Start is called before the first frame update
    void Start()
    {
        Chasingcoeff = 4.9f / jumpforce;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initpos = transform.position;
        player = GameObject.FindGameObjectsWithTag("Player");
        rd=GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.idle:
                animator.Play("Frog_idle");
                break;
            case State.jump:
                animator.Play("frog_jump");
                break;
            case State.fall:
                break;
        }

        if (timer > 2.5f)
        {

            //For Online
            //if(Mathf.Min((player[0].transform.position - transform.position).magnitude,(player[1].transform.position - transform.position).magnitude)<4.0f)
            if ((player[0].transform.position - transform.position).magnitude < 4.0f)
            {
                float speed = (player[0].transform.position.x - transform.position.x) * Chasingcoeff;

                spriteRenderer.flipX = (speed > 0) ? true : false;
                rd.velocity = new Vector2(speed, jumpforce);
                state= State.jump;
            }

            timer = 0f;
        }

        if (state == State.jump)
        {
            if (rd.velocity.y < 2)
            {
                state = State.fall;
            }
        }
        if (state == State.fall)
        {
            if (rd.velocity.magnitude < 0.0001f)
                state = State.idle;
        }
        timer += Time.deltaTime;
    }
}
