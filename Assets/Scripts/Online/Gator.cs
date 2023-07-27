using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gator : MonoBehaviourPun
{
    Rigidbody2D rigidbody2d;
    Vector2 startPos;
    bool right;
    public float speed;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        right = true;
    }

    // Update is called once per frame
    void Update()
    {
        hitRight = Physics2D.Raycast(rigidbody2d.position, Vector3.right * 1, 0.7f, LayerMask.GetMask("Platform"));
        hitLeft = Physics2D.Raycast(rigidbody2d.position, Vector3.right * -1, 0.7f, LayerMask.GetMask("Platform"));
        rigidbody2d.velocity = new Vector2(speed * (right ? 1 : -1), 0);
        if (photonView.IsMine)
            photonView.RPC("flip", RpcTarget.All, right);
        
        flip(right);
        if (transform.position.x > startPos.x + 5f || hitRight.collider)
            right = false;
        else if (transform.position.x < startPos.x - 5f || hitLeft.collider)
            right = true;


    }
    [PunRPC]
    void flip(bool right)
    {
        spriteRenderer.flipX = right;
    }
    public void DestroyEnemy()
    {
        Debug.Log("시발 좀 되라");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
