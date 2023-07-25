using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Offline_UI_Respawn : MonoBehaviour
{
    public TMP_Text Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer.text = ((int)TutorialGameManager.instance.RespawnTime).ToString();
    }
}
