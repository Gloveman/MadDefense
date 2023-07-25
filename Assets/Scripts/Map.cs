using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField]
    private TMP_Text txtname;
    [SerializeField]
    private TMP_Text txttime;


    private string _name;
    public string mapname
    {
        get => _name;
        set
        {
            _name= value;
            txtname.text= _name;
        }
    }
    public void SetButton(string name, string time)
    {
        mapname = name;
        txttime.text = time;
        GetComponent<Button>().onClick.AddListener(() => { });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
