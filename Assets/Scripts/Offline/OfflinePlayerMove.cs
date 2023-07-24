using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Photon.Realtime;
using Photon.Pun;

public class OfflinePlayerMove : MonoBehaviourPun
{

    public static GameObject LocalPlayerInstance = null;
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D boxCollider2D;

    RaycastHit2D raycastHit2D;

    public float speed = 5; // player �ӷ�
    private float HorizontalInput = 0; // ���� �Է�
    public float jumpForce = 9; // Jump ��
    public float antiGravity = 9.8f;
    private bool jump;

    private enum State { idle, run, jump, fall, hurt }; // idle�� 0, run�� 1 �̷� ������ ������ ���� (enum�� Ư¡)
    private State state = State.idle; // ���� ���´� idle(0)�̴�

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
        
        if (TutorialGameManager.instance.currentGameState == GameState.inGame)
        {

            raycastHit2D = Physics2D.Raycast(rigid2D.position, Vector3.down, 0.7f, LayerMask.GetMask("Platform"));

            Flip(HorizontalInput);
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
            // Debug.Log(animator.GetInteger("state") != (int)state);
            if (animator.GetInteger("state") != (int)state)
                animator.SetInteger("state", (int)state);
        }
    }

    private void FixedUpdate()
    {
        
        //switch (TutorialGameManager.instance.currentGameState)
        //{
        //    case GameState.inGame:
                rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
                //break;
        //}

    }

    private void Move()
    {
        rigid2D.velocity = new Vector2(HorizontalInput * speed, rigid2D.velocity.y);
    }

    private void Jump()
    {
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
    }

    void Flip(float x)
    {
        if (x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}

