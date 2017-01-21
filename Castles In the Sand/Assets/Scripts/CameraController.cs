using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Vector2 zoomLimits;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector2 moveLimits;

    private float yRotation = 0;
    private float curZoom;
    private Transform cameraTransform;
    private Rigidbody body;

    // Use this for initialization
    void Start ()
    {
        cameraTransform = transform.GetChild(0);
        cameraTransform.LookAt(transform);
        curZoom = (zoomLimits.x + zoomLimits.y) * 0.5f;
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        float rotation = Input.GetAxis("Horizontal") * -rotateSpeed * Time.deltaTime;
        float zoom = Input.GetAxis("Vertical") * zoomSpeed * Time.deltaTime;
        float strafe = Input.GetAxis("Strafe") * moveSpeed * Time.deltaTime;

        if(Input.GetButtonUp("Horizontal"))
        {
            body.angularVelocity *= 0.5f;
        }

        body.AddTorque(new Vector3(0, rotation, 0));
        //yRotation -= rotation;
        curZoom -= zoom;
        if(curZoom < zoomLimits.x)
        {
            curZoom = zoomLimits.x;
        }
        if(curZoom > zoomLimits.y)
        {
            curZoom = zoomLimits.y;
        }
        
        transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, moveLimits.x, moveLimits.y));
        body.AddForce(new Vector3(0, 0, -strafe));

        //transform.rotation = Quaternion.Euler(0, yRotation, 0);
        cameraTransform.position = transform.position - (cameraTransform.forward * curZoom);
	}
}
