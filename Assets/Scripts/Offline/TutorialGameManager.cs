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
    respawn,
    gameOver
}
public class TutorialGameManager : MonoBehaviourPunCallbacks
{


    public static TutorialGameManager instance;
    public GameState currentGameState = GameState.menu;
    public int score = 0;
    public float time;
    public GameObject[] UI_Pages;
    public GameObject Player;
    public int PlayerHP = 3;
    public float RespawnTime = 0f;
    public void StartGame()
    {
        SetGameState(GameState.inGame);
        time = 300f;
        Offline_CameraMove.player = Player;
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
        currentGameState = GameState.menu;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(currentGameState.ToString());
        switch (currentGameState)
        {
            case GameState.menu:
                UI_Pages[0].SetActive(true);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(false);
                UI_Pages[3].SetActive(false);

                if (Input.GetButtonDown("Jump"))
                    StartGame();
                break;
            case GameState.inGame:
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(true);
                UI_Pages[2].SetActive(false);
                UI_Pages[3].SetActive(false);

                time -= Time.deltaTime;
                if (PlayerHP == 0)
                {
                    RespawnTime = 5f;
                    SetGameState(GameState.respawn);
                    
                    Player.GetComponent<PolygonCollider2D>().enabled = false;
                }
                if (time < 0)
                    SetGameState(GameState.gameOver);

                break;
            case (GameState.respawn):
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(true);
                UI_Pages[3].SetActive(false);
                RespawnTime -= Time.deltaTime;
                if(RespawnTime < 0)
                {

                    Player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    Player.transform.position = new Vector3(0, 0, 0);
                    Player.GetComponent<PolygonCollider2D>().enabled = true;
                    Debug.Log(Player.GetComponent<PolygonCollider2D>().enabled);
                    PlayerHP = 3;
                    Screen.brightness = 0.1f;
                    SetGameState(GameState.inGame);
                }
                break;
            case (GameState.gameOver):
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(false);
                UI_Pages[3].SetActive(true);
                break;
            
            default:
                break;
        }
    }
    void SetGameState(GameState newGameState)
    {
        if (newGameState == GameState.menu)
        {

        }
        else if (newGameState == GameState.inGame)
        {

        }
        else if (newGameState == GameState.gameOver)
        {

        }
        currentGameState = newGameState;
    }
}
