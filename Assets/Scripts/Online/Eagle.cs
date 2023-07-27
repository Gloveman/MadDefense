using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Eagle : MonoBehaviourPun
{
    Rigidbody2D rigidbody2d;
    Vector2 startPos;
    bool up;
    public float speed;
    RaycastHit2D hitUp;
    RaycastHit2D hitDown;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rigidbody2d = GetComponent<Rigidbody2D>();
        up = true;
    }

    // Update is called once per frame
    void Update()
    {
        hitUp = Physics2D.Raycast(rigidbody2d.position, Vector3.up * 1, 0.7f, LayerMask.GetMask("Platform"));
        hitDown = Physics2D.Raycast(rigidbody2d.position, Vector3.up * -1, 0.7f, LayerMask.GetMask("Platform"));
        rigidbody2d.velocity = new Vector2(0, speed * (up ? 1 : -1));
        if (transform.position.y > startPos.y + 5f || hitUp.collider)
            up = false;
        else if (transform.position.y < startPos.y - 5f || hitDown.collider)
            up = true;

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
