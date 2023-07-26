using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMove : MonoBehaviourPun
{
    Animator animator;
    SpriteRenderer spriteRenderer;
    public float jumpforce = 5.0f;
    Vector3 initpos;
    Rigidbody2D rd;
    GameObject[] player;
    float Chasingcoeff;
    float timer = 0f;
    List<GameObject> Players;
    enum State { idle, jump, fall }
    State state = State.idle;
    // Start is called before the first frame update
    void Start()
    {
        Chasingcoeff = 4.9f / jumpforce;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initpos = transform.position;
        player = GameObject.FindGameObjectsWithTag("Player");
        rd = GetComponent<Rigidbody2D>();
        Players = GameManager.instance.Players;
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (state)
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

        if (GameManager.instance.currentGameState == OnlineGameState.inGame)
        {
            Debug.Log(11111111);
            if (timer > 2.5f)
            {
                Debug.Log("제발" + Players.Count);

                float p1Distance = (Players[0].transform.position.x - transform.position.x);
                float p2Distance = (Players[0].transform.position.x - transform.position.x);
                float distance = Mathf.Min(Mathf.Abs(p1Distance), Mathf.Abs(p2Distance));
                Debug.Log(distance);
                Debug.Log(PhotonNetwork.IsMasterClient);
                if (PhotonNetwork.IsMasterClient && distance < 4.0f)
                {

                    Debug.Log(33333333333);
                    float speed = (Mathf.Abs(p1Distance)<Mathf.Abs(p2Distance))? (p1Distance * Chasingcoeff): (p2Distance * Chasingcoeff);

                    if (speed != 0)
                        photonView.RPC("flip", RpcTarget.AllBuffered, speed);
                    rd.velocity = new Vector2(speed, jumpforce);
                    state = State.jump;
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

    [PunRPC]
    void flip(float x)
    {
        spriteRenderer.flipX = x > 0 ? true : false;
       
    }

    [PunRPC]
    public void DestroyEnemy()
    {
        Debug.Log("시발 좀 되라");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
