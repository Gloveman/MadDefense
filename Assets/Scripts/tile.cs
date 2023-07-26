using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TileType
{
    Empty=0, Tile1, Tile2, Tile3, Tile4,
    Spawn=10, Frog, Eagle, Opossum, Gator,
    Cherry=20,
    End=100
}


public class tile : MonoBehaviour
{
    [SerializeField]
    private Sprite[] tileimgs;

    [SerializeField]
    private Sprite[] enemimgs;

    [SerializeField]
    private Sprite[] itemimgs;

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
            else if((int)tiletype < (int)TileType.Cherry)
                spriteRenderer.sprite = enemimgs[(int)tiletype-(int)TileType.Spawn-1];
            else if ((int)tiletype < (int)TileType.End)
                spriteRenderer.sprite = itemimgs[(int)tiletype - (int)TileType.Cherry];
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
