using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class NewBehaviourScript : MonoBehaviour
{
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D boxCollider2D;

    RaycastHit2D raycastHit2D;

    public float speed = 5; // player 속력
    private float HorizontalInput; // 수평 입력
    public float jumpForce = 9; // Jump 힘
    public float antiGravity = 9.8f;
    private bool jump;

    private enum State { idle, run, jump, fall, hurt }; // idle은 0, run은 1 이런 식으로 순차적 대응 (enum의 특징)
    private State state = State.idle; // 시작 상태는 idle(0)이다

    public GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigid2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if(manager.currentGameState == GameState.inGame)
        {

            raycastHit2D = Physics2D.Raycast(rigid2D.position, Vector3.down, 0.7f, LayerMask.GetMask("Platform"));

            Flip();

            switch (state)
            {
                case State.idle:
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    jump = Input.GetButtonDown("Jump");
                    Move();
                    if (HorizontalInput != 0) state = State.run;
                    if (jump)
                    {
                        state = State.jump;
                        Jump();
                    }
                    break;
                case State.run:
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    jump = Input.GetButtonDown("Jump");
                    Move();
                    if (jump)
                    {
                        Jump();
                        state = State.jump;
                    }
                    if (Math.Abs(rigid2D.velocity.x) < 0.3f) state = State.idle;
                    if (rigid2D.velocity.y < -1f) state = State.fall;
                    break;
                case State.jump:
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    Move();
                    if (rigid2D.velocity.y < 0.5f) state = State.fall;
                    break;
                case State.fall:
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    Move();
                    if (raycastHit2D.collider != null) state = State.idle;
                    break;
                case State.hurt:
                    break;
            }
            Debug.Log(animator.GetInteger("state") != (int)state);
            if (animator.GetInteger("state") != (int)state)
                animator.SetInteger("state", (int)state);
        }
    }

    private void FixedUpdate()
    {
        rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        
       
    }

    private void Move()
    {
        rigid2D.velocity = new Vector2(HorizontalInput * speed , rigid2D.velocity.y);
    }
    private void Flip()
    {
        if (HorizontalInput < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (HorizontalInput > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
    private void Jump()
    {
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
    }
}

