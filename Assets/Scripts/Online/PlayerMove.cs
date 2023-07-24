using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using Photon.Realtime;
using Photon.Pun;
using Unity.VisualScripting;

public class PlayerMove : MonoBehaviourPun
{

    public static GameObject LocalPlayerInstance=null;
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    Animator animator;
    BoxCollider2D boxCollider2D;

    RaycastHit2D raycastHit2D;
    RaycastHit2D sideRayHit;
    Vector2 perp;

    public float speed = 5; // player �ӷ�
    private float HorizontalInput; // ���� �Է�
    public float jumpForce = 9; // Jump ��
    public float antiGravity = 9.8f;
    private bool jump;
    private bool isSlope = false;
    private enum State { idle, run, jump, fall, hurt }; // idle�� 0, run�� 1 �̷� ������ ������ ���� (enum�� Ư¡)
    private State state = State.idle; // ���� ���´� idle(0)�̴�

    public static GameManager manager;
    // Start is called before the first frame update

    private void Awake()
    {
        if (photonView.IsMine)
           LocalPlayerInstance = gameObject;
        DontDestroyOnLoad(gameObject);
    }

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
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        if(manager.currentGameState == OnlineGameState.inGame)
        {

            raycastHit2D = Physics2D.Raycast(rigid2D.position, Vector3.down, 0.7f, LayerMask.GetMask("Platform"));


            getSlope();

            //Flip();
            if (photonView.IsMine&&HorizontalInput!=0)
                photonView.RPC("flip",RpcTarget.AllBuffered, HorizontalInput);

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
                    if (rigid2D.velocity.y < -1f && raycastHit2D.collider == null) state = State.fall;
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
        rigid2D.constraints = RigidbodyConstraints2D.FreezeRotation;
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
        rigid2D.velocity = new Vector2(HorizontalInput * speed , rigid2D.velocity.y);
    }

    private void Jump()
    {
        rigid2D.velocity = new Vector2(rigid2D.velocity.x, jumpForce);
    }

    [PunRPC]
    void flip(float x)
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
