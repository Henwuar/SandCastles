using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    [SerializeField]
    float rippleSpeed;
    [SerializeField]
    float maxWaveHeight;
    [SerializeField]
    float waveSpeed;
    [SerializeField]
    float tideSpeed;
    [SerializeField]
    float maxTide;
    [SerializeField]
    float erosionPower;



    private Material material;
    private float timer = 0;
    private float waterLevel = 0;
    private float targetWaveHeight;
    private float waveHeight = 0;
    private float curScale = 1;
    private float targetScale = 1;

	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Renderer>().material;
        Camera.main.depthTextureMode = DepthTextureMode.Depth;
        targetWaveHeight = Random.Range(0.01f, maxWaveHeight);
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        timer += Time.deltaTime;
        material.SetFloat("_Timer", timer*rippleSpeed);
        waveHeight = targetWaveHeight * Mathf.Sin(waveSpeed * (timer % (2*Mathf.PI)));
        //if(waveHeight > 0)
        //{
            waterLevel += tideSpeed * Time.deltaTime;
        //}

        if(waterLevel >= 0.5f || waterLevel < 0)
        {
            tideSpeed *= -1;
        }
        if(waveHeight > -0.001f && waveHeight < 0.001f)
        {
            targetWaveHeight = Random.Range(0.01f, maxWaveHeight);
        }
        
        transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Min(waterLevel + waveHeight, maxTide), 5);

        Vector3 startPoint = new Vector3(transform.parent.position.x, transform.parent.position.y, 5);
        Vector3 endPoint = new Vector3(transform.parent.position.x + 5, transform.parent.position.y, 5);
        Ray ray = new Ray(startPoint, new Vector3(1, 0, 0));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if(hit.collider && hit.collider.tag == "Sand")
        {
            endPoint = hit.point;
            //transform.position = Vector3.Lerp(transform.parent.position, hit.point, 0.5f);
            targetScale = (Vector3.Distance(startPoint, endPoint) / 10) + 0.1f;
            
            hit.collider.gameObject.GetComponent<Sand>().Erode(hit.point.x, erosionPower + erosionPower * (waterLevel/maxTide));

            
        }
        else
        {
            targetScale = 1;
        }

        curScale = Mathf.Lerp(curScale, targetScale, 2 * Time.deltaTime);
        transform.parent.localScale = new Vector3(curScale, 1, transform.parent.localScale.z);
    }

    public float GetRippleTime()
    {
        return timer * rippleSpeed;
    }
}
