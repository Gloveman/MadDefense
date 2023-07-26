using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Photon.Realtime;
using Photon.Pun;
using Unity.VisualScripting;

public class Offline_Player2 : MonoBehaviourPun
{

    [SerializeField]
    AudioClip jumpsound;

    public static GameObject LocalPlayerInstance = null;
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    Animator animator;


    RaycastHit2D raycastHit2D;
    RaycastHit2D sideRayHit;
    Vector2 perp;

    public float speed = 5; // player 속력
    private float HorizontalInput = 0; // 수평 입력
    public float jumpForce = 9; // Jump 힘
    public float antiGravity = 9.8f;
    private bool jump;
    private bool isJumping = false;
    private bool isSlope = false;

    public float fallfrom;

    public enum State { idle, run, jump, fall, hurt }; // idle은 0, run은 1 이런 식으로 순차적 대응 (enum의 특징)
    public State state = State.idle; // 시작 상태는 idle(0)이다

    // Start is called before the first frame update


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rigid2D = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        if (TutorialGameManager.instance.currentGameState == GameState.inGame)
        {

            raycastHit2D = Physics2D.Raycast(rigid2D.position, Vector3.down, 0.7f, LayerMask.GetMask("Platform"));
            getSlope();

            Flip(HorizontalInput);
            if (fallfrom - transform.position.y > 10f) TutorialGameManager.instance.PlayerHP = 0;

            switch (state)
            {
                case State.idle:
                    isJumping = false;
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    jump = Input.GetButtonDown("Jump");
                    fallfrom = transform.position.y;
                    Move();
                    if (HorizontalInput != 0) state = State.run;
                    if (jump)
                    {
                        state = State.jump;
                        GetComponent<AudioSource>().PlayOneShot(jumpsound);
                    }
                    break;
                case State.run:
                    isJumping = false;
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    jump = Input.GetButtonDown("Jump");
                    Move();
                    fallfrom = transform.position.y;
                    if (jump)
                    {
                        state = State.jump;
                        GetComponent<AudioSource>().PlayOneShot(jumpsound);
                    }
                    if (rigid2D.velocity.y < -1f && raycastHit2D.collider == null) state = State.fall;
                    break;
                case State.jump:

                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    Move();
                    fallfrom = transform.position.y;
                    if (!isJumping)
                    {
                        Jump();
                        isJumping = true;
                    }

                    if (rigid2D.velocity.y < 0.3f && raycastHit2D.collider != null) state = State.idle;
                    break;
                case State.fall:
                    isJumping = false;
                    HorizontalInput = Input.GetAxisRaw("Horizontal");
                    Move();
                    if (raycastHit2D.collider != null) state = State.idle;
                    break;
                case State.hurt:
                    Debug.Log(state);

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

        if (isSlope)
        {

            if (HorizontalInput == 0)
            {
                if (state == State.idle)
                    rigid2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            }
            if (state == State.idle || state == State.run)
            {
                rigid2D.velocity = perp * speed * HorizontalInput * -1;
            }
            Debug.Log(rigid2D.velocity);
        }

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
    void getSlope()
    {
        sideRayHit = Physics2D.Raycast(rigid2D.position, Vector2.down, 1, LayerMask.GetMask("Platform"));
        if (Vector2.Angle(sideRayHit.normal, Vector2.up) != 0)
        {
            isSlope = true;
            perp = Vector2.Perpendicular(sideRayHit.normal).normalized;
        }
        else isSlope = false;
    }
}

