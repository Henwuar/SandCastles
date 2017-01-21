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

    float curSand = 0;

	// Use this for initialization
	void Start ()
    {
		
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
            cursor.transform.position = hit.point;
            float cursorScale = brushSize * 1.0f / hit.collider.GetComponent<Terrain>().terrainData.size.magnitude;
            cursor.localScale = new Vector3(cursorScale, cursorScale, cursorScale);
        }
        else
        {
            cursor.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0) && curSand > 0)
        {
            if (hit.collider && hit.collider.gameObject.tag == "Sand")
            {
                sand.Paint(hit.point, brushSize);
                curSand -= Time.deltaTime;
            }
        }
        else if(Input.GetMouseButton(1))
        {
            if (hit.collider && hit.collider.gameObject.tag == "Sand")
            {
                sand.Paint(hit.point, brushSize, false);
                curSand += Time.deltaTime;
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
        brushSize = Mathf.Clamp(brushSize, 2, 20);
    }
}
