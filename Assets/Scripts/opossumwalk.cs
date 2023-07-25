using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class opossumwalk : MonoBehaviour
{
    Rigidbody2D rigid2D;
    SpriteRenderer spriteRenderer;
    private float speed = 3;
    private int nextMove = -1;
    // Start is called before the first frame update
    void Start()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        rigid2D.velocity = new Vector2(nextMove * speed, rigid2D.velocity.y);
        Vector2 frontVec = new Vector2(rigid2D.position.x + nextMove * (0.5f + Mathf.Epsilon), rigid2D.position.y);
        Vector2 belowVec = new Vector2(rigid2D.position.x, rigid2D.position.y - 0.3f + Mathf.Epsilon);
        //ºûÀ¸·Î Platform °¨Áö
        RaycastHit2D rayHitDown = Physics2D.Raycast(frontVec, Vector2.down, 2, LayerMask.GetMask("Platform"));
        RaycastHit2D rayHitSide = Physics2D.Raycast(belowVec, new Vector2(rigid2D.velocity.normalized.x, 0), 0.5f, LayerMask.GetMask("Platform"));

        if (rayHitDown.collider == null || rayHitSide.collider != null)
        {
            if (nextMove > 0) spriteRenderer.flipX = false;
            else spriteRenderer.flipX = true;
            nextMove *= -1;
        }
    }
}
