using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class UI_GameClear : MonoBehaviourPun
{
    public bool isloaded;
    public TMP_Text p1_basescore;
    public TMP_Text p2_basescore;
    public TMP_Text p1_bonusscore;
    public TMP_Text p2_bonusscore;
    public TMP_Text p1_totalscore;
    public TMP_Text p2_totalscore;
    public TMP_Text p1_result;
    public TMP_Text p2_result;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(GameManager.instance.scoreloaded);
        if (GameManager.instance.scoreloaded == true)
        {
            p1_totalscore.text = (int.Parse(p1_basescore.text) + int.Parse(p1_bonusscore.text)).ToString();
            p2_totalscore.text = (int.Parse(p2_basescore.text) + int.Parse(p2_bonusscore.text)).ToString();
            if (int.Parse(p1_totalscore.text) > int.Parse(p2_totalscore.text))
            {
                p1_result.text = "W";
                p2_result.text = "L";
            }
            else
            {
                p1_result.text = "L";
                p2_result.text = "W";
            }
        }
        
       // score.text = GameManager.instance.score.ToString();
    }

}
