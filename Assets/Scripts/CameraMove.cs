using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private static GameObject player;
    public Vector3 offset = new Vector3(0, 0, -10);
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    public static void setplayer(GameObject obj)
    {
        player = obj;
    }
    void Update()
    {
        this.transform.position = player.transform.position + offset;
        if (transform.position.y < -4f)
        {
            transform.position = new Vector3(transform.position.x, -4f, transform.position.z);
        }
        if (transform.position.y > 4f)
        {
            transform.position = new Vector3(transform.position.x, 4f, transform.position.z);
        }
    }
}
