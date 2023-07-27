using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offline_Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    float time = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 0.13f)
            gameObject.SetActive(false);
    }
}
