using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opossumwalk : MonoBehaviourPun
{
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    private float speed = 3;
    private int nextMove = -1;
    bool isok = false;
    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        Vector2 frontVec = new Vector2(rigid2D.position.x + nextMove * (0.8f + Mathf.Epsilon), rigid2D.position.y);
        Vector2 belowVec = new Vector2(rigid2D.position.x, rigid2D.position.y - 0.3f + Mathf.Epsilon);
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        Debug.DrawRay(belowVec, new Vector2(rigid2D.velocity.normalized.x, 0), new Color(1, 0, 0));
        
        //������ Platform ����
        RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector2.down, 0.5f, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHitSide = Physics2D.Raycast(belowVec, new Vector2(nextMove, 0), 0.5f, LayerMask.GetMask("Platform"));
        Debug.Log(rayHitDown.collider == null || rayHitSide.collider != null);
         if (rayHitDown.collider == null || rayHitSide.collider != null)
         {
            photonView.RPC("flip", RpcTarget.AllBuffered, nextMove);
            nextMove *= -1;

         }
        rigid2D.velocity = new Vector2(nextMove * speed, rigid2D.velocity.y);

    }

    [PunRPC]
    void flip(int x)
    {
        spriteRenderer.flipX = x > 0 ? false : true;

    }

    [PunRPC]
    public void DestroyEnemy()
    {
        Debug.Log("�ù� �� �Ƕ�");
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
