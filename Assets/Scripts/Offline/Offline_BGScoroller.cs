using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offline_BGSScoroller : MonoBehaviour
{
    private MeshRenderer render;
    private float offsetX;
    private float offsetY;
    public GameObject Player;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(TutorialGameManager.instance.currentGameState == GameState.inGame)
        {
            this.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, 100);
            offsetX = (new Vector3(0, 0, 0) - this.transform.position).x;
            offsetY = (new Vector3(0, 0, 0) - this.transform.position).y;
            render.material.mainTextureOffset = new Vector2(offsetX * speed, offsetY * speed * -1);
        }
    }
}
