using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Offline_UI_GameClear : MonoBehaviour
{
    public TMP_Text score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score.text = TutorialGameManager.instance.score.ToString();
    }
}
