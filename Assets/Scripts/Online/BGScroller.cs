using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour
{
    private MeshRenderer render;
    private float offsetX;
    private float offsetY;
    public GameObject Player;
    public float speedX;
    public float speedY;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.currentGameState == OnlineGameState.inGame)
        {
            this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, 100);
            offsetX = (new Vector3(0, 0, 0) - this.transform.position).x;
            offsetY = (new Vector3(0, 0, 0) - this.transform.position).y;
            render.material.mainTextureOffset = new Vector2(offsetX * speedX, offsetY * speedY * -1);
        }
    }
}
