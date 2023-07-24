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

public enum OnlineGameState
{
    waiting,
    menu,
    inGame,
    gameOver
}
public class GameManager : MonoBehaviourPunCallbacks
{
    private bool isparsed = false;
    private bool isloaded = false;
    public Tilemap map;
    public Tile testone;
    private WebSocket ws;
    private List<Vector3Int> points;
    private List<Tile> tiles;
    private string rawmap;


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
            ws.OnMessage += delegate (object sender, MessageEventArgs e)
            {
                rawmap = e.Data;
                var parsed = JObject.Parse(e.Data);

                foreach (JObject item in parsed["data"])
                {
                    int x = int.Parse(item["points"][0].ToString());
                    int y = int.Parse(item["points"][1].ToString());
                    points.Add(new Vector3Int(x, y, 0));
                }

                foreach (JObject item in parsed["data"])
                {
                    tiles.Add(testone);
                }
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
                int x = int.Parse(item["points"][0].ToString());
                int y = int.Parse(item["points"][1].ToString());
                points.Add(new Vector3Int(x, y, 0));
            }

            foreach (JObject item in parsed["data"])
            {
                tiles.Add(testone);
            }
            isparsed = true;
        }

        if (PlayerMove.LocalPlayerInstance == null)
        {
            Player = PhotonNetwork.Instantiate("Player", new Vector3(0, 4, 0), Quaternion.identity, 0);
            Debug.Log(Camera.main.GetComponent<CameraMove>().player);
            Camera.main.GetComponent<CameraMove>().player = Player;
            Debug.Log(Camera.main.GetComponent<CameraMove>().player);
            Debug.Log("Added player");
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
