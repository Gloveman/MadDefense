using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using WebSocketSharp;
using Newtonsoft.Json.Linq;
using System.Linq;
using Unity.VisualScripting;

struct MapInfo
{
    public string Name;
    public string Time;
    public string Level;
    public MapInfo(string name, string time,string level)
    {
        Name = name;
        Time = time;
        Level = level;
    }
}
public class MapSelect : MonoBehaviourPunCallbacks
{
    List<MapInfo> mapinfos;
    bool isConnecting=false;
    bool isparsed = false;
    bool isloaded = false;
    private byte maxPlayersPerRoom = 2;
    
    string selectedroom;

    [SerializeField]
    private GameObject buttonprefab;

    [SerializeField]
    private GameObject addbuttonprefab;

    [SerializeField]
    private GameObject Content;


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

    }

    // Start is called before the first frame update
    void Start()
    {
        //Get map data
        WebSocket ws = new WebSocket("ws://127.0.0.1:5001");
        ws.Connect();
        var req = new JObject
        {
            { "ContentType", "selectmap" }
        };
        ws.Send(req.ToString());
        ws.OnMessage += Ws_OnMessage;
        
    }

    public void Goback()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        mapinfos = new List<MapInfo>();
        var parsed = JObject.Parse(e.Data);
        for(int i = 0; i < ((JArray)parsed["names"]).Count;i++)
        {
            mapinfos.Add(new MapInfo(parsed["names"][i].ToString(), parsed["times"][i].ToString(), parsed["levels"][i].ToString()));
        }
        isparsed = true;

    }

    // Update is called once per frame
    void Update()
    {
        if(isparsed&&!isloaded)
        {
            for (int i = 0; i < mapinfos.Count; i++)
            {
                GameObject mapbtn = Instantiate(buttonprefab);
                mapbtn.transform.SetParent(Content.transform);
                //var btnimg = mapbtn.GetComponent<Image>();
                //btnimg.sprite = ;
                mapbtn.GetComponent<Map>().SetButton(mapinfos[i].Name, mapinfos[i].Time);
                mapbtn.GetComponent<Button>().onClick.AddListener(() => { Connect(mapbtn.GetComponent<Map>().mapname); });
            }
            //add map editor button
            GameObject addbtn = Instantiate(addbuttonprefab);
            addbtn.transform.SetParent(Content.transform);
            addbtn.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("Map Editor"); });

            Content.GetComponent<RectTransform>().anchoredPosition = new Vector2(30+(mapinfos.Count+1) * 700 - 1920, 0);
            isloaded = true;
        }
    }

    public void Connect(string selectedroom)
    {
        isConnecting = true;
        this.selectedroom = selectedroom;
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRoom(selectedroom);
        }
        else
        {
            PhotonNetwork.GameVersion = "1";
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to master");
        Debug.Log(isConnecting);
        if (isConnecting)
            PhotonNetwork.JoinRoom(selectedroom);
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("disconnected because of {0}", cause);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("creating new room....");
        PhotonNetwork.CreateRoom(selectedroom, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room joined");
        PhotonNetwork.LoadLevel("SampleScene");

    }
}
