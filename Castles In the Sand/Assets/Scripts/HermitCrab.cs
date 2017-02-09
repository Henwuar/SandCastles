using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitCrab : MonoBehaviour
{
    public Sand sand;

    [SerializeField]
    private Vector2 bounds;
    [SerializeField]
    private float arriveDistance;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private Water water;

    private Vector3 avoid;
    private Vector3 target;
    private float waitTime;
    private Rigidbody body;
    private Animator animator;

	// Use this for initialization
	void Start ()
    {
        body = GetComponent<Rigidbody>();
        GetNewTarget();
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(transform.position, new Vector3(target.x, transform.position.y, target.z)) < arriveDistance)
            {
                body.velocity = Vector3.zero;
                waitTime = Random.Range(0.1f, 5.0f);
                GetNewTarget();
            }
            else
            {
                transform.position = new Vector3(transform.position.x, sand.GetHeight(transform.position), transform.position.z);
                //RaycastHit hit;
                /*Physics.Raycast(transform.position, -Vector3.up, out hit);
                if(hit.collider && hit.collider.tag == "Sand")
                {
                    transform.position = hit.point;

                }
                else if(hit.collider && hit.collider.tag == "Water")
                {
                    transform.position = hit.point - Vector3.up * transform.position.x / 10;
                }*/
                transform.LookAt(transform.position + Vector3.Lerp(transform.forward, target - transform.position, Time.deltaTime), sand.GetNormal(transform.position));
                body.AddForce(transform.forward * acceleration);
                body.velocity = Vector3.ClampMagnitude(body.velocity, maxSpeed);

            }
        }
        animator.SetFloat("Speed", body.velocity.magnitude / maxSpeed);
	}

    void GetNewTarget()
    {
        target = new Vector3(Random.Range(bounds.x, bounds.y), transform.position.y, Random.Range(bounds.x, bounds.y));
    }
}
