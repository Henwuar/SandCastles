using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSandWalls : MonoBehaviour
{
    private TerrainData terrain;

	// Use this for initialization
	void Start ()
    {
        terrain = GetComponent<Terrain>().terrainData;	
	}

    public void CreateWalls()
    {
        Vector3[] vertices = new Vector3[10];
        Vector2[] uvs = new Vector2[10];

        float[,] heights = terrain.GetHeights(0, 0, terrain.heightmapWidth, 1);
        int flattenX = terrain.heightmapWidth;
        for(int x = 0; x < terrain.heightmapWidth; x++)
        {
            if(heights[0,x] < Sand.BASE_HEIGHT)
            {
                flattenX = x;
            }
            else
            {
                break;
            }
        }

        //Vector3 flattenPoint = new Vector3(flattenX, Sand.BASE_HEIGHT * terrain.size.y);
        float yMax = Sand.BASE_HEIGHT * terrain.size.y;
        float xMax = terrain.size.x;
        float flattenPoint = ((float)flattenX / terrain.heightmapWidth)*terrain.size.x;
        float zMax = terrain.size.z;

        vertices[0] = Vector3.zero;
        vertices[1] = new Vector3(flattenPoint, 0);
        vertices[2] = new Vector3(flattenPoint, yMax);
        vertices[3] = new Vector3(xMax, yMax);
        vertices[4] = new Vector3(xMax, 0);
        vertices[5] = new Vector3(xMax, 0, zMax);
        vertices[6] = new Vector3(xMax, yMax, zMax);
        vertices[7] = new Vector3(flattenPoint, yMax, zMax);
        vertices[8] = new Vector3(flattenPoint, 0, zMax);
        vertices[9] = new Vector3(0, 0, zMax);

        for(int i = 0; i < 10; i++)
        {
            uvs[i] = Vector2.zero;
        }

        int[] tris = new int[]
        {
            0, 2, 1,
            2, 3, 1,
            1, 3, 4,
            3, 5, 4,
            3, 6, 5,
            6, 8, 5,
            6, 7, 8,
            7, 9, 8
        };

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = tris;
    }
}
