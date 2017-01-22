using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scenery : MonoBehaviour
{
    [SerializeField]
    private bool maintainRotation = false;

	// Update is called once per frame
	void Update ()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, -Vector3.up, out hit);
        if (hit.collider)
        {
            if (hit.collider.gameObject.tag == "Sand")
            {
                if (transform.position.y > hit.point.y)
                {
                    transform.position = hit.point;
                    if(!maintainRotation)
                    {
                        Vector3 lookAtPoint = hit.point + Vector3.Cross(hit.normal, transform.right);
                        transform.LookAt(lookAtPoint);
                    }
                }
            }
        }
    }
}
