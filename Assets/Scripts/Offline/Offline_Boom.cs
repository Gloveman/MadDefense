using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offline_Boom : MonoBehaviour
{
    float time = 0f;
    public GameObject Explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 3f)
        {
            Debug.Log(Instantiate(Explosion, transform.position, new Quaternion()));
            RaycastHit2D[] rayHits = Physics2D.CircleCastAll(transform.position, 2, new Vector3(0, 0, 0));
            for(int i = 0; i < rayHits.Length; i++)
            {
                GameObject colliderObject = rayHits[i].collider.gameObject;
                if (colliderObject.tag == "Player")
                {
                    colliderObject.GetComponent<Offline_CollisionControl>().OnDamaged(transform.position);
                }
                else if (colliderObject.tag == "Player2")
                {
                    colliderObject.GetComponent<Offline_CollisionControl2>().OnDamaged(transform.position);
                }
                else if (colliderObject.tag == "Enemy")
                {
                    colliderObject.SetActive(false);
                    
                }
            }
            gameObject.SetActive(false);
        }
        
    }
}