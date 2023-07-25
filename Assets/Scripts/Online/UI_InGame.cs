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
    // Start is called before the first frame update
    void Start()
    {
        Heart = Resources.Load<Sprite>("Heart");
        EmptyHeart = Resources.Load<Sprite>("EmptyHeart");
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
    }
}
