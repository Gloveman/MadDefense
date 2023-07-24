using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    [SerializeField]
    private GameObject tilePrefab;

    [SerializeField]
    private TMP_InputField inputwidth;
    [SerializeField]
    private TMP_InputField inputheight;

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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
