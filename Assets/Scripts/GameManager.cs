using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    menu,
    inGame,
    gameOver
}
public class GameManager : MonoBehaviour
{

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
        currentGameState = GameState.menu;
    }

    // Update is called once per frame
    void Update()
    {
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
}
