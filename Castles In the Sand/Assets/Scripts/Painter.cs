using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painter : MonoBehaviour
{
    [SerializeField]
    private Sand sand;
    [SerializeField]
    private int brushSize;
    [SerializeField]
    Transform cursor;
    [SerializeField]
    float maxSand;
    [SerializeField]
    float sandFloat;
    [SerializeField]
    private int maxBrushSize;

    private Transform shadow;
    private Transform sandBall;

    private float shadowScale;
    private float ballScale;

    float curSand = 0;

	// Use this for initialization
	void Start ()
    {
        shadow = cursor.transform.FindChild("Shadow");
        sandBall = cursor.transform.FindChild("SandBall");
        shadowScale = cursor.transform.localScale.x;
        ballScale = cursor.transform.localScale.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if(hit.collider && hit.collider.gameObject.tag == "Sand")
        {
            cursor.gameObject.SetActive(true);
            cursor.transform.position = hit.point + Vector3.up;
            shadow.position = hit.point + Vector3.up*0.01f;
            //Vector3 lookAtPoint = hit.point + hit.normal;
            //cursor.transform.LookAt(lookAtPoint);
            float curShadowScale = brushSize * (1.0f / hit.collider.GetComponent<Terrain>().terrainData.size.x) * 0.25f ;
            shadow.localScale = new Vector3(curShadowScale, curShadowScale, curShadowScale);
        }
        else
        {
            cursor.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0) && curSand > 0)
        {
            if (hit.collider && hit.collider.gameObject.tag == "Sand")
            {
                sand.Paint(hit.point, brushSize, maxBrushSize);
                curSand -= Time.deltaTime * brushSize;
            }
        }
        if(Input.GetMouseButton(1))
        {
            if (hit.collider && hit.collider.gameObject.tag == "Sand")
            {
                sand.Paint(hit.point, brushSize, maxBrushSize, false);
                curSand += Time.deltaTime * brushSize;
            }
        }
        if(Input.GetMouseButton(2))
        {
            if (hit.collider && hit.collider.gameObject.tag == "Sand")
            {
                sand.Smooth(hit.point, brushSize);
                curSand += Time.deltaTime * brushSize;
            }
        }

        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            brushSize++;
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            brushSize--;
        }
        brushSize = Mathf.Clamp(brushSize, 2, maxBrushSize);

        float ballSize = Mathf.Clamp((curSand / maxSand), 0.1f, 1);
        sandBall.localScale = new Vector3(ballSize, ballSize, ballSize);
    }
}
