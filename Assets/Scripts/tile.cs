using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Empty=0, Tile1, Tile2, Tile3, Tile4,
    Spawn=10,
    End=100
}


public class tile : MonoBehaviour
{
    [SerializeField]
    private Sprite[] tileimgs;
    [SerializeField]
    private Sprite spawnimg;
    [SerializeField]
    private Sprite endimg;

    private TileType tiletype;

    private SpriteRenderer spriteRenderer;

    public TileType Tiletype
    {
        set
        {
            tiletype= value;
            if ((int)tiletype < (int)TileType.Spawn)
            {
                spriteRenderer.sprite = tileimgs[(int)tiletype];
            }
            else if ((int)tiletype == (int)TileType.Spawn)
                spriteRenderer.sprite = spawnimg;
            else
                spriteRenderer.sprite = endimg;
        }
        get => tiletype;
    }


    public void Setup(TileType type)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Tiletype = type;

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
