using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Offline_ItemControl : MonoBehaviour
{
    private Queue<GameObject> ItemList;
    private float clearTime;
    // Start is called before the first frame update
    void Start()    
    {
        ItemList = new Queue<GameObject>(); 
    }

    // Update is called once per frame
    void Update()
    {
        itemProcess();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(TutorialGameManager.instance.currentGameState == GameState.inGame)
        {
            ItemList.Enqueue(collision.gameObject);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Endpoint")
        {
            Debug.Log("in Endpoint " + clearTime.ToString());
            clearTime += Time.deltaTime;
            
            if(clearTime > 3)
            { 
                TutorialGameManager.instance.currentGameState = GameState.gameOver;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Endpoint")
        {
            Debug.Log("Out Endpoint " +  clearTime.ToString());
            clearTime += Time.deltaTime;
            if (clearTime > 3)
            {
                clearTime = 0;

            }
        }
    }
    private void itemProcess()
    {
        for(int i = 0; i < ItemList.Count; i++)
        {
            GameObject collisionObject = ItemList.Dequeue();
            if(!collisionObject.activeSelf) {
                continue;
            }
            else
            {
                if (collisionObject.tag == "Item")
                {

                    Debug.Log(2);
                    //점수
                    bool isCherry = collisionObject.name.Contains("Cherry");
                    if (isCherry)
                    {

                        Debug.Log(1);
                        TutorialGameManager.instance.score += 100;
                    }
                    //아이템 사라짐
                    collisionObject.SetActive(false);


                }
            }
        }
    }
}
