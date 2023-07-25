using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using UnityEngine.Tilemaps;
using Photon.Pun;
using Newtonsoft.Json.Linq;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Unity.VisualScripting;

public enum OnlineGameState
{
    waiting,
    menu,
    inGame,
    gameOver
}

struct MobInfo
{
    public Vector3 pos;
    public TileType type;
    public MobInfo(float x, float y, TileType mobtype)
    {
        pos = new Vector3(x, y, 0);
        type = mobtype;
    }
}

public class GameManager : MonoBehaviourPunCallbacks
{
    private bool isparsed = false;
    private bool isloaded = false;
    public Tilemap map;
    private WebSocket ws;
    private List<Vector3Int> points;
    private List<Tile> tiles;
    private List<MobInfo> mobs;
    private string rawmap;

    public Tile[] TileArray;
    private Vector3 SpawnPoint;
    public static GameManager instance;
    public OnlineGameState currentGameState = OnlineGameState.waiting;
    public int score = 0;
    public float time;
    public GameObject[] UI_Pages;
    public GameObject Player;

    public void StartGame()
    {

        SetOnlineGameState(OnlineGameState.inGame);
        time = 300f;
    }

    public void GameOver()
    {

    }
    
    public void BackToMenu()
    {

    }
    private void Awake()
    {
        instance = this;
        PlayerMove.manager = this;
    }

    
    // Start is called before the first frame update
    void Start()
    {

        map.ClearAllTiles();
        points = new List<Vector3Int>();
        tiles = new List<Tile>();
        if (PhotonNetwork.IsMasterClient)
        {
            ws = new WebSocket("ws://127.0.0.1:5001");
            ws.Connect();
            var req = new JObject
            {
                { "name", PhotonNetwork.CurrentRoom.Name }
            };
            ws.Send(req.ToString());
            ws.OnMessage += delegate (object sender, MessageEventArgs e)
            {
                Debug.Log("Get map");
                rawmap = e.Data;
                var parsed = JObject.Parse(e.Data);

                foreach (JObject item in parsed["data"])
                {
                    TileType type = (TileType)int.Parse(item["tile"].ToString());
                    int x = int.Parse(item["points"][0].ToString());
                    int y = int.Parse(item["points"][1].ToString());

                    if (type < TileType.Spawn)
                    {
                        tiles.Add(TileArray[(int)type - 1]);
                        points.Add(new Vector3Int(x, y, 0));
                    }
                    else if (type == TileType.Spawn)
                    {
                        Debug.Log("Spawn point");
                        SpawnPoint = new Vector3(x,y,0);
                    }
                    else if (type < TileType.End)
                    {
                        mobs.Add(new MobInfo(x, y, type));
                    }
                    else
                    {

                    }
                    
                }

               for(int i=0;i<points.Count;i++)
                {
                    var pre = points[i];
                    points[i] = new Vector3Int(pre.x - (int)SpawnPoint.x,(int)SpawnPoint.y-pre.y,0);
                }

               for(int i=0;i<mobs.Count;i++)
                {
                    var pre = mobs[i];
                    mobs[i] = new MobInfo(pre.pos.x-SpawnPoint.x,SpawnPoint.y-pre.pos.y,pre.type);
                }


                SpawnPoint = Vector3.zero;
                isparsed = true;

                
            };
        }
        else
        {
            Debug.Log("Slave client came");
            rawmap = PhotonNetwork.CurrentRoom.CustomProperties["rawmap"].ToString();
            var parsed = JObject.Parse(rawmap);

            foreach (JObject item in parsed["data"])
            {
                TileType type = (TileType)int.Parse(item["tile"].ToString());
                int x = int.Parse(item["points"][0].ToString());
                int y = int.Parse(item["points"][1].ToString());

                if (type < TileType.Spawn)
                {
                    tiles.Add(TileArray[(int)type - 1]);
                    points.Add(new Vector3Int(x, y, 0));
                }
                else if (type == TileType.Spawn)
                {

                    SpawnPoint = new Vector3(x, y, 0);
                }

                else if(type<TileType.End)
                {
                    
                }
                else
                {

                }
            }
            for (int i = 0; i < points.Count; i++)
            {
                var pre = points[i];
                points[i] = new Vector3Int(pre.x - (int)SpawnPoint.x, (int)SpawnPoint.y - pre.y, 0);
            }
            SpawnPoint = Vector3.zero;
            isparsed = true;

        }




        currentGameState = OnlineGameState.waiting;
    }

    // Update is called once per frame
    void Update()
    {
        if (isparsed && !isloaded)
        {
            map.SetTiles(points.ToArray(), tiles.ToArray());
            Debug.Log("Loaded map");

            if(PhotonNetwork.IsMasterClient)
            {
                foreach (MobInfo mob in mobs)
                {
                    //PhotonNetwork.Instantiate(,) later

                }
            }

            if (PlayerMove.LocalPlayerInstance == null)
            {
                Player = PhotonNetwork.Instantiate("Player", SpawnPoint, Quaternion.identity, 0);
                Debug.Log(Camera.main.GetComponent<CameraMove>().player);
                Camera.main.GetComponent<CameraMove>().player = Player;
                Debug.Log(Camera.main.GetComponent<CameraMove>().player);
                Debug.Log("Added player");
            }
            isloaded = true;
            if (PhotonNetwork.IsMasterClient) { 
            Hashtable currentmap = new Hashtable { { "rawmap", rawmap } };
            PhotonNetwork.CurrentRoom.SetCustomProperties(currentmap);
            Debug.Log("property updated");
            }
        }

        //Debug.Log(currentGameState.ToString());
        switch (currentGameState)
        {
            case OnlineGameState.waiting:
                UI_Pages[0].SetActive(true);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(false);
                UI_Pages[3].SetActive(false);
                Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount + 1000000000000000);
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
                    StartGame();
                break;
            case OnlineGameState.menu:
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(true);
                UI_Pages[2].SetActive(false);
                UI_Pages[3].SetActive(false);
                if (Input.GetButtonDown("Jump"))
                    StartGame();
                break;
            case OnlineGameState.inGame:
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(true);
                UI_Pages[3].SetActive(false);
                break;
            case (OnlineGameState.gameOver):
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(false);
                UI_Pages[3].SetActive(true);
                break;
            default:
                break;
        }
    }

    void SetOnlineGameState(OnlineGameState newGameState)
    {
        if(newGameState == OnlineGameState.waiting)
        {

        }
        else if(newGameState == OnlineGameState.menu)
        {
            
        }
        else if(newGameState == OnlineGameState.inGame)
        {

        }else if(newGameState == OnlineGameState.gameOver)
        {

        }
        currentGameState = newGameState;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
            LoadScene();
    }

    private void LoadScene()
    {
        PhotonNetwork.LoadLevel("SampleScene");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
