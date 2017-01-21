using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    [SerializeField]
    private float digSpeed;

    private Texture2D heightMap;
    private TerrainData terrain;

    private const float BASE_HEIGHT = 0.25f;

    float timer = 0;

	// Use this for initialization
	void Start ()
    {
        terrain = GetComponent<Terrain>().terrainData;
        
        ResetHeight();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    public void Paint(Vector3 position, int size, bool additive = true)
    {
        Vector3 point = position - transform.position;
        int xBase = Mathf.FloorToInt((point.x / terrain.size.x) * terrain.heightmapWidth) - size/2;
        int yBase = Mathf.FloorToInt((point.z / terrain.size.z) * terrain.heightmapHeight) - size / 2;

        if (xBase < 0)
        {
            xBase = 0;
        }
        if(yBase < 0)
        {
            yBase = 0;
        }

        int w = size;
        if(xBase + w > terrain.heightmapWidth)
        {
            w = terrain.heightmapWidth - xBase;
            print(w);
        }
        int h = size;
        if(yBase + h > terrain.heightmapHeight)
        {
            h = terrain.heightmapHeight - yBase;
        }

        float addVal = additive ? digSpeed : -digSpeed;

        float[,] heights = terrain.GetHeights(xBase, yBase, w, h);

        for(int i = 0; i < w; i++)
        {
            for(int j = 0; j < h; j++)
            {
                float xVal = i / size;
                float yMin = 0.0f;
                float yMax = 1.0f;
                if(heights[h / 2, w / 2] > BASE_HEIGHT)
                {
                    yMin = 0.25f;
                }
                else if(heights[h/2, w/2] < BASE_HEIGHT)
                {
                    yMax = 1.0f;
                }

               
                Vector2 circlePos = new Vector2(Mathf.Sin(((float)i/size)*Mathf.PI), Mathf.Sin(((float)j/size))*Mathf.PI);
                heights[j, i] = Mathf.Clamp(heights[j, i] + addVal * Time.deltaTime * circlePos.magnitude, yMin, yMax);
            }
        }

        terrain.SetHeights(xBase, yBase, heights);
    }

    void ResetHeight()
    {
        float[,] resetHeight = new float[terrain.heightmapHeight, terrain.heightmapHeight];
        for (int x = 0; x < terrain.heightmapWidth; x++)
        {
            for (int y = 0; y < terrain.heightmapHeight; y++)
            {
                resetHeight[y, x] = BASE_HEIGHT;
            }
        }
        terrain.SetHeights(0, 0, resetHeight);
    }

    void OnApplicationQuit()
    {
        ResetHeight();
    }
}
