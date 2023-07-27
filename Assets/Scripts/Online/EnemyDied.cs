using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDied : MonoBehaviourPun
{
    float time;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine) return;

        time += Time.deltaTime;
        if (time > 0.15f)
            PhotonNetwork.Destroy(photonView.gameObject);
    }
}
