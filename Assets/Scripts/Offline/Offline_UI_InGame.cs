using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Offline_UI_InGame : MonoBehaviour
{
    public TMP_Text UIPoint;
    public TMP_Text UITimer;
    public Image[] UIHeart;
    Sprite Heart;
    Sprite EmptyHeart;
    
    public Image[] InventoryImage;
    public Offline_ItemControl itemControl;
    // Start is called before the first frame update
    void Start()
    {
        Heart = Resources.Load<Sprite>("Heart");
        EmptyHeart = Resources.Load<Sprite>("EmptyHeart");
        itemControl = TutorialGameManager.instance.Player.GetComponent<Offline_ItemControl>();
    }

    // Update is called once per frame
    void Update()
    {

        UIPoint.text = "Score: " + TutorialGameManager.instance.score.ToString();
        int minute = (int)TutorialGameManager.instance.time / 60;
        int second = (int)TutorialGameManager.instance.time % 60;
        UITimer.text = minute.ToString("00") + ":" + second.ToString("00");


        int HP = TutorialGameManager.instance.PlayerHP;
        for(int i = 0; i < HP; i++)
            UIHeart[i].sprite = Heart;
        for (int i = HP; i < 3; i++)
            UIHeart[i].sprite = EmptyHeart;

        InventoryImage[0].sprite = itemControl.Inventory[0] == 0 ? null : itemControl.UsableItemSprites[itemControl.Inventory[0]];
        InventoryImage[1].sprite = itemControl.Inventory[1] == 0 ? null : itemControl.UsableItemSprites[itemControl.Inventory[1]];
    }
}
