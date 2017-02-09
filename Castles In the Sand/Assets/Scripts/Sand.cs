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
    [SerializeField]
    private float smoothSpeed;

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

    public void Paint(Vector3 position, int size, int maxSize, bool additive = true)
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
        addVal *= Mathf.Clamp((maxSize/size)*5, 0.1f, 1.0f);
        print(addVal);

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

    public void Erode(float heightPoint, float maxPower)
    {
        //print("eroding");
        float pos = heightPoint - transform.position.x;
        int sampleWidth = Mathf.FloorToInt((pos / terrain.size.x) * terrain.heightmapWidth);

        float[,] heights = terrain.GetHeights(0, 0, sampleWidth, terrain.heightmapHeight);

        float power = maxPower;
        for(int i = 0; i < sampleWidth; i++)
        {
            float targetHeight = GetResetHeight(i);
            for (int j = 0; j < terrain.heightmapHeight; j++)
            {
                float heightDiff = Mathf.Abs(heights[j, i] - targetHeight);
                if (heightDiff <= 0.01f)
                {
                    heights[j, i] = targetHeight;
                }
                else
                {
                    float erosionModifier = 1;
                    power--;
                    if(heightDiff > BASE_HEIGHT*0.5f)
                    {
                        erosionModifier = 2;
                    }
                    heights[j, i] = Mathf.Lerp(heights[j, i], targetHeight, erosionRate * erosionModifier * Time.deltaTime);
                }
                if(power <= 0)
                {
                    break;
                }
            }
        }
        terrain.SetHeights(0, 0, heights);
    }

    public void Smooth(Vector3 position, int size)
    {
        Vector3 point = position - transform.position;
        int xBase = Mathf.FloorToInt((point.x / terrain.size.x) * terrain.heightmapWidth) - size / 2;
        int yBase = Mathf.FloorToInt((point.z / terrain.size.z) * terrain.heightmapHeight) - size / 2;

        if (xBase <= 0)
        {
            xBase = 1;
        }
        if (yBase <= 0)
        {
            yBase = 1;
        }

        int w = size;
        if (xBase + w >= terrain.heightmapWidth)
        {
            w = terrain.heightmapWidth - xBase - 1;
        }
        int h = size;
        if (yBase + h >= terrain.heightmapHeight)
        {
            h = terrain.heightmapHeight - yBase - 1;
        }

        float[,] heights = terrain.GetHeights(xBase, yBase, w, h);

        float averageHeight = 0;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                averageHeight += heights[j, i];
            }
        }

        averageHeight /= (w * h);

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                heights[j, i] = Mathf.Lerp(heights[j, i], averageHeight, Time.deltaTime*smoothSpeed);
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

    public float GetHeight(Vector3 position)
    {
        Vector3 point = position - transform.position;
        int xBase = Mathf.FloorToInt((point.x / terrain.size.x) * terrain.heightmapWidth);
        int yBase = Mathf.FloorToInt((point.z / terrain.size.z) * terrain.heightmapHeight);

        return terrain.GetHeight(xBase, yBase);
    }   

    public Vector3 GetNormal(Vector3 position)
    {
        Vector3 point = position - transform.position;
        int xBase = Mathf.FloorToInt((point.x / terrain.size.x) * terrain.heightmapWidth);
        int yBase = Mathf.FloorToInt((point.z / terrain.size.z) * terrain.heightmapHeight);

        return terrain.GetInterpolatedNormal(xBase, yBase);
    }
}
