using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Respawn : MonoBehaviour
{
    public TMP_Text Timer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        Timer.text = ((int)GameManager.instance.RespawnTime).ToString();
    }
}
