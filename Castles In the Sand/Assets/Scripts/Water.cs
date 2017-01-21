using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private Material material;
    private float timer = 0;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Renderer>().material;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;
        material.SetFloat("_Timer", timer);
	}
}
