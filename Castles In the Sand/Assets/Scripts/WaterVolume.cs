using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterVolume : MonoBehaviour
{
	// Update is called once per frame
	void Update ()
    {
        transform.localScale = new Vector3(10, transform.parent.position.y * 2.4f, 10);
        transform.localPosition = new Vector3(transform.localPosition.x/*+(transform.parent.localScale.x*0.1f)*/, -transform.parent.position.y, transform.localPosition.z);
	}
}
