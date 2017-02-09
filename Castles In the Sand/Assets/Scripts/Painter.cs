using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private AudioSource audio;
    public AudioClip smooth;
    public AudioClip move;

    private Transform shadow;
    private Transform sandBall;

    private float shadowScale;
    private float ballScale;

    private GameObject lastHit = null;
    private GameObject selected = null;

    float curSand = 0;

	// Use this for initialization
	void Start ()
    {
        shadow = cursor.transform.FindChild("Shadow");
        sandBall = cursor.transform.FindChild("SandBall");
        shadowScale = cursor.transform.localScale.x;
        ballScale = cursor.transform.localScale.x;
        audio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if(hit.collider && hit.collider.gameObject.tag == "Sand" && selected == null)
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

        if (hit.collider)
        {
            if(hit.collider.gameObject.tag == "Sand")
            {
                if (Input.GetMouseButton(0))
                {
                    if (curSand > 0)
                    {
                        audio.clip = move;
                        if (!audio.isPlaying)
                        {
                            audio.Play();
                        }
                        sand.Paint(hit.point, brushSize, maxBrushSize);
                        curSand -= Time.deltaTime * brushSize;
                    }
                }
                else if (Input.GetMouseButton(1))
                {
                    audio.clip = move;
                    if (!audio.isPlaying)
                    {
                        audio.Play();
                    }
                    sand.Paint(hit.point, brushSize, maxBrushSize, false);
                    
                    curSand += Time.deltaTime * brushSize;
                    
                }
                else if (Input.GetMouseButton(2))
                {
                    audio.clip = smooth;
                    if(!audio.isPlaying)
                    {
                        audio.Play();
                    }
                    
                    sand.Smooth(hit.point, brushSize); 
                }
                else
                {
                    audio.Stop();
                }
            }
            if (hit.collider.gameObject.tag == "Interactable" && selected == null)
            {
                lastHit = hit.collider.gameObject;
                if(lastHit.GetComponent<Interactable>().CanHighlight())
                {
                    lastHit.GetComponent<Interactable>().SetHighlighted(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        selected = lastHit;
                        selected.layer = LayerMask.NameToLayer("Ignore Raycast");
                    }
                }
            }
            else
            {
                if(lastHit != null)
                {
                    lastHit.GetComponent<Interactable>().SetHighlighted(false);
                }
                lastHit = null;
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

        if (hit.collider && hit.collider.gameObject.tag == "Sand" && selected != null)
        {
            selected.transform.position = hit.point;
            selected.GetComponent<Interactable>().SetHighlighted(true);
            Vector3 lookAtPoint = hit.point + Quaternion.Euler(0, 0, 90) * hit.normal;
            selected.transform.LookAt(lookAtPoint);
            if (Input.GetMouseButtonDown(0))
            {
                print("drop");
                selected.layer = LayerMask.NameToLayer("Default");
                selected.GetComponent<Interactable>().SetHighlighted(true);
                selected = null;
            }
        }
    }
}
