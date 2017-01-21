using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float minZoom;
    [SerializeField]
    private float maxZoom;
    [SerializeField]
    private float zoomSpeed;

    private float yRotation = 0;
    private float curZoom;
    private Transform cameraTransform;
    private Rigidbody body;

    // Use this for initialization
    void Start ()
    {
        cameraTransform = transform.GetChild(0);
        cameraTransform.LookAt(transform);
        curZoom = (maxZoom + minZoom) * 0.5f;
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float rotation = Input.GetAxis("Horizontal") * -rotateSpeed * Time.deltaTime;
        float zoom = Input.GetAxis("Vertical") * zoomSpeed * Time.deltaTime;

        if(Input.GetButtonUp("Horizontal"))
        {
            body.angularVelocity *= 0.5f;
        }

        body.AddTorque(new Vector3(0, rotation, 0));
        //yRotation -= rotation;
        curZoom -= zoom;
        if(curZoom < minZoom)
        {
            curZoom = minZoom;
        }
        if(curZoom > maxZoom)
        {
            curZoom = maxZoom;
        }

        //transform.rotation = Quaternion.Euler(0, yRotation, 0);
        cameraTransform.position = transform.position - (cameraTransform.forward * curZoom);
	}
}
