using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemControl : MonoBehaviourPun
{
    [SerializeField]
    AudioClip Itemsound;
    private Queue<GameObject> ItemList;
    private float clearTime;
    public int[] Inventory;

    private PlayerMove PlayerMove;
    public Sprite[] UsableItemSprites;
    public string[] UsableItems = new string[]{ "", "BoomUsed" };
    // Start is called before the first frame update
    void Start()
    {
        ItemList = new Queue<GameObject>();
        Debug.Log(ItemList);
        Inventory = new int[2];
        Inventory[0] = 0;
        Inventory[1] = 0;
        PlayerMove = GameManager.instance.Player.GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine && PhotonNetwork.IsConnected)
            return;
        itemProcess();
        Debug.Log(Input.GetButtonDown("Fire1") + "후후후후후");
        if (Input.GetButtonDown("Fire1") && (PlayerMove.state != PlayerMove.State.hurt) && Inventory[0] != 0)
        {
            Debug.Log(999999999999999999);
            Debug.Log(UsableItems[Inventory[0]]);
            PhotonNetwork.Instantiate(UsableItems[Inventory[0]], transform.position, new Quaternion());
            Debug.Log(10000000000);
            Inventory[0] = 0;
        }
        if (Input.GetButtonDown("Fire2") && (PlayerMove.state != PlayerMove.State.hurt) && Inventory[1] != 0)
        {
            Debug.Log(999999999999999999);
            PhotonNetwork.Instantiate(UsableItems[Inventory[1]], transform.position, new Quaternion());
            Inventory[1] = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.currentGameState == OnlineGameState.inGame)
        {
            
            Debug.Log(ItemList);
            ItemList.Enqueue(collision.gameObject);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {

            if (GameManager.instance.currentGameState == OnlineGameState.inGame)
            {
                if (collision.tag == "Endpoint")
                {
                    Debug.Log("in Endpoint " + clearTime.ToString());
                    clearTime += Time.deltaTime;

                    if (clearTime > 3)
                    {
                        GameManager.instance.iscleared = true;
                        photonView.RPC("GameClear", RpcTarget.All);
                    //GameManager.instance.currentGameState = OnlineGameState.gameClear;
                }
                }
            }
        

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

            if (GameManager.instance.currentGameState == OnlineGameState.inGame)
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
        for (int i = 0; i < ItemList.Count; i++)
        {
            Debug.Log("Point 1");
            GameObject collisionObject = ItemList.Dequeue();
            if (collisionObject.IsDestroyed())
            {
                Debug.Log("Point 2");
                continue;
            }
            else
            {
                if (collisionObject.tag == "Item")
                {

                    Debug.Log("Point 3");
                    //점수
                    bool isCherry = collisionObject.name.Contains("Cherry");
                    if (isCherry)
                    {
                        if (photonView.IsMine)
                            photonView.RPC("ItemSound", RpcTarget.All);
                        Debug.Log("Point 4");
                        GameManager.instance.score += 100;
                    }
                    if (collisionObject.name.Contains("Boom"))
                    {
                        if (photonView.IsMine)
                            photonView.RPC("ItemSound", RpcTarget.All);
                        if (Inventory[0] == 0)
                            Inventory[0] = 1;
                        else if (Inventory[1] == 0)
                            Inventory[1] = 1;
                    }
                    //아이템 사라짐
                    //collisionObject.SetActive(false);
                    photonView.RPC("DestroyItem", RpcTarget.MasterClient,collisionObject.GetPhotonView().ViewID);
                }
            }
        }
    }

    [PunRPC]
    void ItemSound()
    {
        GetComponent<AudioSource>().PlayOneShot(Itemsound);
    }


    [PunRPC]
    public void DestroyItem(int viewid)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            var itemobj = PhotonView.Find(viewid);
            PhotonNetwork.Destroy(itemobj);
        }
    }

    [PunRPC]
    public void GameClear()
    {

        GameManager.instance.currentGameState = OnlineGameState.gameClear;
    }
}
