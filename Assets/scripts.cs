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
        ws = new WebSocket("ws://127.0.0.1:5001");
        ws.Connect();
        ws.Send("From unity!");
        ws.OnMessage += ws_OnOpen;
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
