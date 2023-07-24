using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class movecamera : MonoBehaviour
{
    [SerializeField]
    private TileMap tilemap;
    private Camera mainCamera;

    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float minViewSize = 2;
    private float maxViewSize;

    private float wDelta = 0.4f;
    private float hDelta = 0.6f;

    private void Awake()
    {
        mainCamera = GetComponent<Camera>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetupCamera()
    {
        int width = tilemap.Width;
        int height = tilemap.Height;

        float size = (width > height) ? width * wDelta : height * hDelta;
        mainCamera.orthographicSize = size;

        if (height > width)
        {
            Vector3 pos = new Vector3(0, 0.05f, -10);
            pos.y *= height;

            transform.position = pos;
        }
        maxViewSize = mainCamera.orthographicSize;

    }
    public void SetPosition(float x,float y)
    {
        transform.position += new Vector3(x, y, 0) * moveSpeed * Time.deltaTime;
    }
    public void SetOrthographicSize(float size)
    {
        if (size == 0) return;

        mainCamera.orthographicSize += size * zoomSpeed * Time.deltaTime;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minViewSize, maxViewSize);
    }
}
