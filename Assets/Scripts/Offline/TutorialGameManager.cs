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

public class TutorialGameManager : MonoBehaviourPunCallbacks
{


    public static TutorialGameManager instance;
    public GameState currentGameState = GameState.menu;
    public int score = 0;
    public float time;
    public GameObject[] UI_Pages;
    public GameObject Player;
    public void StartGame()
    {
        SetGameState(GameState.inGame);
        time = 300f;
        CameraMove.player = Player;
        Rigidbody2D playerRigidbody2D = Player.GetComponent<Rigidbody2D>();
        playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
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
        //Rigidbody2D playerRigidbody2D = Player.GetComponent<Rigidbody2D>();
        //playerRigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
        //Debug.Log(playerRigidbody2D.constraints.ToString());
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

                
                if (Input.GetButtonDown("Jump"))
                    StartGame();
                break;
            case GameState.inGame:
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(true);
                UI_Pages[2].SetActive(false);
                time -= Time.deltaTime;
                break;
            case (GameState.gameOver):
                UI_Pages[0].SetActive(false);
                UI_Pages[1].SetActive(false);
                UI_Pages[2].SetActive(true);
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
