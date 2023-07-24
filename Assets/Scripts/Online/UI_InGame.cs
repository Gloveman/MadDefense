using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class UI_InGame : MonoBehaviour
{
    public TMP_Text UIPoint;
    public TMP_Text UITimer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = "Score: " + GameManager.instance.score.ToString();
        int minute = (int)GameManager.instance.time / 60;
        int second = (int)GameManager.instance.time % 60;
        UITimer.text = minute.ToString("00") + ":" + second.ToString("00");
    }
}
