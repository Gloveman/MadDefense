using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Offline_ItemControl : MonoBehaviour
{
    [SerializeField]
    AudioClip itemsound;

    [SerializeField]
    AudioClip boomsound;

    private Queue<GameObject> ItemList;
    private float clearTime;
    private Offline_PlayerMove offline_PlayerMove;
    public int[] Inventory;
    public Sprite[] UsableItemSprites;
    public GameObject[] UsableItems;
    float timer = 0;
    bool timer_start = false;
    // Start is called before the first frame update
    void Start()    
    {
        ItemList = new Queue<GameObject>(); 
        Inventory = new int[2];
        Inventory[0] = 0;
        Inventory[1] = 0;
        offline_PlayerMove = gameObject.GetComponent<Offline_PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        itemProcess();
        if (timer>3.1f)
        {
            GetComponent<AudioSource>().PlayOneShot(boomsound);
            timer = 0;
            timer_start = false;
        }
            Debug.Log(Input.GetButtonDown("Fire1") + "후후후후후");
        if (Input.GetButtonDown("Fire1") && (offline_PlayerMove.state != Offline_PlayerMove.State.hurt) && Inventory[0] != 0)
        {
            Debug.Log(999999999999999999);
            if (Inventory[0] == 1)
            {
                Instantiate(UsableItems[Inventory[0]], transform.position, new Quaternion());
                timer_start = true;
            }
            else if (Inventory[0] == 2)
                offline_PlayerMove.superjump();
            Inventory[0] = 0;
        }
        if (Input.GetButtonDown("Fire2") && (offline_PlayerMove.state != Offline_PlayerMove.State.hurt) && Inventory[1] != 0)
        {
            Debug.Log(999999999999999999);
            if (Inventory[1] == 1)
            {
                timer_start = true;
                Instantiate(UsableItems[Inventory[1]], transform.position, Quaternion.identity);
            }
            else if (Inventory[1] == 2)
                offline_PlayerMove.superjump();
            Inventory[1] = 0;
        }
        if (timer_start)
            timer += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(11111111111111111);
        if(TutorialGameManager.instance.currentGameState == GameState.inGame)
        {
            if(collision.gameObject.tag=="Item")
                GetComponent<AudioSource>().PlayOneShot(itemsound);
            ItemList.Enqueue(collision.gameObject);
        }
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(TutorialGameManager.instance.currentGameState == GameState.inGame) { 
            if(collision.tag == "Endpoint")
            {
                Debug.Log("in Endpoint " + clearTime.ToString());
                clearTime += Time.deltaTime;
                
                if(clearTime > 3)
                { 
                    TutorialGameManager.instance.currentGameState = GameState.gameClear;
                }
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (TutorialGameManager.instance.currentGameState == GameState.inGame)
        {
            if (collision.tag == "Endpoint")
            {
                Debug.Log("Out Endpoint " + clearTime.ToString());
                clearTime += Time.deltaTime;
                if (clearTime > 3)
                {
                    clearTime = 0;

                }
            }
        }
    }
    private void itemProcess()
    {
        Debug.Log(ItemList.Count);
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
                    if (collisionObject.name.Contains("Cherry"))
                    {

                        Debug.Log(1);
                        TutorialGameManager.instance.score += 100;
                    }
                    //아이템 사라짐

                    if (collisionObject.name.Contains("Boom"))
                    {
                        if (Inventory[0] == 0)
                            Inventory[0] = 1;
                        else if (Inventory[1] == 0)
                            Inventory[1] = 1;
                    }
                    if (collisionObject.name.Contains("spring"))
                    {
                        if (Inventory[0] == 0)
                            Inventory[0] = 2;
                        else if (Inventory[1] == 0)
                            Inventory[1] = 2;
                    }
                    collisionObject.SetActive(false);
                }
            }
        }
    }
}
