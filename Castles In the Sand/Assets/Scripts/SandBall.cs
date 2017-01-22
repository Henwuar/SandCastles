using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandBall : MonoBehaviour
{
    [SerializeField]
    private float rippleSpeed;

    private float timer = 0;

    Material material;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Renderer>().material;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer += Time.deltaTime;

        material.SetFloat("_Timer", timer * rippleSpeed);	
	}
}
