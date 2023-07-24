using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class scripts : MonoBehaviour
{
    private WebSocket ws;
    // Start is called before the first frame update
    void Start()
    {

    }

    void ws_OnOpen(object sender, MessageEventArgs e)
    {
        Debug.Log(e.Data);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
