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

public enum GameState
{
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
    public GameState currentGameState = GameState.menu;

    public void StartGame()
    {
        SetGameState(GameState.inGame);
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




        currentGameState = GameState.menu;
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

        Debug.Log(currentGameState.ToString());
        switch (currentGameState)
        {
            case GameState.menu:
                bool button = Input.GetButtonDown("Jump");
                Debug.Log(button);
                if (button)
                    StartGame();
                break;
            case GameState.inGame:
                break;
            case (GameState.gameOver):
                break;
            default:
                break;
        }
    }

    void SetGameState(GameState newGameState)
    {
        if(newGameState == GameState.menu)
        {
            
        }
        else if(newGameState == GameState.inGame)
        {

        }else if(newGameState == GameState.gameOver)
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
        PhotonNetwork.LoadLevel("GameScene");
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
