using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemControll : MonoBehaviour
{
    BoxCollider2D BoxCollider2D;
    private Queue<GameObject> ItemList;
    
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
