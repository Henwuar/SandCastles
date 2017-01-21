using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
    [SerializeField]
    private Transform waterContainer;
    private Water water;
    private Material material;

    void Start()
    {
        water = waterContainer.GetComponentInChildren<Water>();
        material = GetComponent<Renderer>().material;
    }

	// Update is called once per frame
	void Update ()
    { 
        transform.parent.localScale = new Vector3(waterContainer.localScale.x, waterContainer.position.y*2.1f, waterContainer.localScale.z);
        material.SetFloat("_Timer", water.GetRippleTime());
	}
}
