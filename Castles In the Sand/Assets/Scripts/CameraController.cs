using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private Vector2 rotationLimits;
    [SerializeField]
    private Vector2 zoomLimits;
    [SerializeField]
    private float zoomSpeed;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private Vector4 moveLimits;
    [SerializeField]
    private Sprite orbitImage;
    public Sprite panImage;
    public GameObject icon;

    private float yRotation = 0;
    private float zRotation = 0;
    private float curZoom;
    private Transform cameraTransform;
    private Rigidbody body;
    [SerializeField]
    private bool orbit = false;

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
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            orbit = !orbit;
        }


        
        
        if(orbit)
        {
            icon.GetComponent<Image>().sprite = orbitImage; 
            /*if (Input.GetButtonUp("Horizontal") && orbit)
            {
                body.angularVelocity *= 0.5f;
            }*/
            float hor = Input.GetAxis("Horizontal") * -rotateSpeed * Time.deltaTime;
            float ver = Input.GetAxis("Vertical") * -rotateSpeed * Time.deltaTime;
            float zoom = Input.GetAxis("Strafe") * zoomSpeed * Time.deltaTime;

            body.AddTorque(new Vector3(0, hor, ver));
            if (transform.rotation.eulerAngles.z < rotationLimits.x + 360 && transform.rotation.eulerAngles.z > 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationLimits.x);
                body.angularVelocity = new Vector3(body.angularVelocity.x, body.angularVelocity.y, 0);
            }
            if (transform.rotation.eulerAngles.z > rotationLimits.y && transform.rotation.eulerAngles.z < 180)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, rotationLimits.y);
                body.angularVelocity = new Vector3(body.angularVelocity.x, body.angularVelocity.y, 0);
            }

            //yRotation -= rotation;
            curZoom -= zoom;
            if (curZoom < zoomLimits.x)
            {
                curZoom = zoomLimits.x;
            }
            if (curZoom > zoomLimits.y)
            {
                curZoom = zoomLimits.y;
            }
        }
        else
        {
            icon.GetComponent<Image>().sprite = panImage;
            float hor = Input.GetAxis("Horizontal") * -moveSpeed * Time.deltaTime;
            float ver = Input.GetAxis("Vertical") * -moveSpeed * Time.deltaTime;
            float zoom = Input.GetAxis("Strafe") * moveSpeed*0.25f * Time.deltaTime;
            
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, moveLimits.x, moveLimits.y), Mathf.Clamp(transform.position.y, moveLimits.z, moveLimits.w), Mathf.Clamp(transform.position.z, moveLimits.x, moveLimits.y));
            Vector3 adjForward = new Vector3(-cameraTransform.forward.x, 0, -cameraTransform.forward.z);
            Vector3 adjRight = new Vector3(-cameraTransform.right.x, 0, -cameraTransform.right.z);
            body.AddForce(adjForward * ver + adjRight * hor + Vector3.up*zoom);
        }


        cameraTransform.LookAt(cameraTransform.position + cameraTransform.forward, Vector3.up);


        //transform.rotation = Quaternion.Euler(0, yRotation, 0);
        cameraTransform.position = transform.position - (cameraTransform.forward * curZoom);
	}
}
