using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sand : MonoBehaviour
{
    [SerializeField]
    private float digSpeed;
    [SerializeField]
    private float slope;
    [SerializeField]
    private float erosionRate;

    private Texture2D craterMap;
    private TerrainData terrain;

    public const float BASE_HEIGHT = 0.25f;

    float timer = 0;

	// Use this for initialization
	void Start ()
    {
        terrain = GetComponent<Terrain>().terrainData;
        
        ResetHeight();

        GetComponent<CreateSandWalls>().CreateWalls();
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

        if (xBase <= 0)
        {
            xBase = 1;
        }
        if(yBase <= 0)
        {
            yBase = 1;
        }

        int w = size;
        if(xBase + w >= terrain.heightmapWidth)
        {
            w = terrain.heightmapWidth - xBase -1;
        }
        int h = size;
        if(yBase + h >= terrain.heightmapHeight)
        {
            h = terrain.heightmapHeight - yBase -1;
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

    public void Erode(float heightPoint)
    {
        //print("eroding");
        float pos = heightPoint - transform.position.x;
        int sampleWidth = Mathf.FloorToInt((pos / terrain.size.x) * terrain.heightmapWidth);

        float[,] heights = terrain.GetHeights(0, 0, sampleWidth, terrain.heightmapHeight);
        for(int i = 0; i < sampleWidth; i++)
        {
            float targetHeight = GetResetHeight(i);
            for (int j = 0; j < terrain.heightmapHeight; j++)
            {
                heights[j, i] = Mathf.Lerp(heights[j, i], targetHeight, erosionRate*Time.deltaTime);
            }
        }

        terrain.SetHeights(0, 0, heights);
    }

    void ResetHeight()
    {
        float[,] resetHeight = new float[terrain.heightmapHeight, terrain.heightmapHeight];
        for (int x = 0; x < terrain.heightmapWidth; x++)
        {
            for (int y = 0; y < terrain.heightmapHeight; y++)
            {
                resetHeight[y, x] = GetResetHeight(x);
            }
        }
        terrain.SetHeights(0, 0, resetHeight);
    }

    void OnApplicationQuit()
    {
        ResetHeight();
    }

    float GetResetHeight(int x)
    {
        return Mathf.Min(slope * ((float)x / terrain.heightmapWidth), BASE_HEIGHT);
    }
}
