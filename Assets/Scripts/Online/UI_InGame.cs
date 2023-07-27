using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    public TMP_Text UIPoint;
    public TMP_Text UITimer;
    public Image[] UIHeart;
    Sprite Heart;
    Sprite EmptyHeart;

    public Image[] InventoryImage;
    public ItemControl itemControl;
    // Start is called before the first frame update
    void Start()
    {
        Heart = Resources.Load<Sprite>("Heart");
        EmptyHeart = Resources.Load<Sprite>("EmptyHeart");
        itemControl = GameManager.instance.Player.GetComponent<ItemControl>();
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = "Score: " + GameManager.instance.score.ToString();
        int minute = (int)GameManager.instance.time / 60;
        int second = (int)GameManager.instance.time % 60;
        UITimer.text = minute.ToString("00") + ":" + second.ToString("00");

        int HP = GameManager.instance.PlayerHP;
        for (int i = 0; i < HP; i++)
            UIHeart[i].sprite = Heart;
        for (int i = HP; i < 3; i++)
            UIHeart[i].sprite = EmptyHeart;

        Debug.Log(itemControl.Inventory[0] + "조조노ㅗ노조노노");
        InventoryImage[0].sprite = itemControl.Inventory[0] == 0 ? null : itemControl.UsableItemSprites[itemControl.Inventory[0]];
        InventoryImage[1].sprite = itemControl.Inventory[1] == 0 ? null : itemControl.UsableItemSprites[itemControl.Inventory[1]];
    }
}
