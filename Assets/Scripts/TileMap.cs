using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using WebSocketSharp;

public class TileMap : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private TMP_InputField inputwidth;
    [SerializeField]
    private TMP_InputField inputheight;

    [SerializeField]
    private TMP_InputField inputname;

    [SerializeField]
    private TMP_InputField inputlevel;

    [SerializeField]
    private TMP_InputField inputlimit;

    public int Width { private set; get; } = 30;
    public int Height { private set; get; } = 20;
    // Start is called before the first frame update

    private void Awake()
    {
        inputwidth.text = Width.ToString();
        inputheight.text = Height.ToString();
        
    }

    private void SpawnTile(TileType type, Vector3 pos)
    {
       GameObject clone=Instantiate(tilePrefab,pos,Quaternion.identity);
        clone.name = "Tile";
        clone.transform.SetParent(transform);
        tile tile = clone.GetComponent<tile>();
        tile.Setup(type);
    }

    public void GenerateMap()
    {
        foreach (Transform t in transform)
            Destroy(t.gameObject);
        int width, height;
        int.TryParse(inputwidth.text, out width);
        int.TryParse(inputheight.text, out height);

        Width = width;
        Height = height;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Vector3 pos = new Vector3((-Width * 0.5f + 0.5f) + x, (Height * 0.5f - 0.5f) - y, 0);
                SpawnTile(TileType.Empty, pos);
            }
        }
    }

    public void Savemap()
    {
        WebSocket ws=new WebSocket("ws://127.0.0.1:5001");
        ws.Connect();
        var json=new JObject();
        string Filename=inputname.text.ToString();
        string Timelimit=inputlimit.text.ToString();
        string level=inputlevel.text.ToString();
        json.Add("ContentType", "savemap");
        json.Add("name",Filename);
        json.Add("time",Timelimit);
        json.Add("level",level);

        var jarray=new JArray();
        foreach (Transform t in transform)
        {
            if(t.gameObject.GetComponent<tile>().Tiletype!=TileType.Empty)
            {
            var elem=new JObject();
            var point=new JArray();

            int x= (int)Mathf.Floor(t.position.x);
            int y=(int)Mathf.Floor(t.position.y);
            point.Add(x); 
            point.Add(y);
            elem.Add("points",point);

            int type=(int)t.gameObject.GetComponent<tile>().Tiletype;
            elem.Add("tile",type);

            jarray.Add(elem);
            }
        }
        json.Add("data",jarray);
        ws.Send(json.ToString());
        Debug.Log("Sended");
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
