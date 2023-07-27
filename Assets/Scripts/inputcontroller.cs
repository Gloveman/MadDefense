using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputcontroller : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    private TileType curtype = TileType.Empty;
    private tile spawntile=null;
    private tile endtile = null;

    [SerializeField]
    private movecamera cameraController;
    private Vector2 premousepostion;
    private Vector2 curmouseposition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        UpdateCamera();
        RaycastHit hit;
        if(Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit,Mathf.Infinity)) {

                tile tile = hit.transform.GetComponent<tile>();
                if(tile != null)
                {
                    if(curtype==TileType.Spawn)
                    {
                        if(spawntile!=null)
                        {
                            spawntile.Tiletype = TileType.Empty;
                        }
                        spawntile = tile;
                    }
                    if (curtype == TileType.End)
                    {
                        if (endtile != null)
                        {
                            endtile.Tiletype = TileType.Empty;
                        }
                        endtile = tile;
                    }
                        tile.Tiletype = curtype;
                }
            
            }
        }
        else if(Input.GetMouseButton(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {

                tile tile = hit.transform.GetComponent<tile>();
                if (tile != null)
                {
                    
                    tile.Tiletype = TileType.Empty;
                }

            }
        }
    }

    private void UpdateCamera()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        cameraController.SetPosition(x, y);

        if(Input.GetMouseButtonDown(2))
        {
            curmouseposition = premousepostion = Input.mousePosition;
        }
        else if(Input.GetMouseButton(2))
        {
            curmouseposition = Input.mousePosition;
            if(premousepostion!=curmouseposition)
            {
                Vector2 move = (premousepostion - curmouseposition) * 0.5f;
                cameraController.SetPosition(move.x, move.y);
            }
        }
        premousepostion = curmouseposition;

        float distance = Input.GetAxisRaw("Mouse ScrollWheel");
        cameraController.SetOrthographicSize(-distance);
    }

    public void SetTileType(int type)
    {
        curtype = (TileType)type;
    }
}
