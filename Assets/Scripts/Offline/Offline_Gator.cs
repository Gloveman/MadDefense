using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Offline_Gator : MonoBehaviour
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
        flip(right);
        if (transform.position.x > startPos.x + 5f || hitRight.collider)
            right = false;
        else if (transform.position.x < startPos.x - 5f || hitLeft.collider)
            right = true;
        
        
    }
    void flip(bool right)
    {
        spriteRenderer.flipX = right;
    }
}
