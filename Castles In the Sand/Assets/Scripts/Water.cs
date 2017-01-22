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
    [SerializeField]
    private GameObject[] spawnables;
    [SerializeField]
    private float spawnChance;


    private Material material;
    private float timer = 0;
    private float waterLevel = 0;
    private float targetWaveHeight;
    private float waveHeight = 0;
    private float curScale = 1;
    private float targetScale = 1;
    private float averageHitpoint;
    private float curStep = 1;
    private float prevHeight;
    private float prevVelocity;


	// Use this for initialization
	void Start ()
    {
        material = GetComponent<Renderer>().material;
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

        if(waterLevel >= maxTide || waterLevel < 0)
        {
            tideSpeed *= -1;
        }
        if(waveHeight > -0.001f && waveHeight < 0.001f)
        {
            targetWaveHeight = Random.Range(0.01f, maxWaveHeight);
        }
        
        transform.parent.position = new Vector3(transform.parent.position.x, Mathf.Min(waterLevel + waveHeight, maxTide), 5);

        Vector3 startPoint = new Vector3(transform.parent.position.x, transform.parent.position.y, curStep-1);
        curStep++;
        if(curStep > 10)
        {
            curStep = 1;
            averageHitpoint = 0;
        }
        Vector3 endPoint = new Vector3(transform.parent.position.x + 5, transform.parent.position.y, 5);
        Ray ray = new Ray(startPoint, new Vector3(1, 0, 0));
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if(hit.collider)
        {
            
            endPoint = hit.point;
            averageHitpoint += hit.point.x;
            //transform.position = Vector3.Lerp(transform.parent.position, hit.point, 0.5f);
            targetScale = ((averageHitpoint/curStep) / 10) + 0.1f;

            if (hit.collider.tag == "Sand")
            {
                hit.collider.gameObject.GetComponent<Sand>().Erode(averageHitpoint / curStep, erosionPower + erosionPower * (waterLevel / maxTide));
            }
            

            
        }
        else
        {
            targetScale = 1;
        }

        float velocity = Mathf.Sign((waterLevel+waveHeight) - prevHeight);
        if(velocity < 0 && prevVelocity >= 0)
        {
            if(Random.Range(0, 100) < spawnChance)
            {
                Vector3 spawnPos = new Vector3((9.8f * transform.parent.localScale.x), prevHeight + 0.1f, Random.Range(1.0f, 9.0f));
            
                int index = Random.Range(0, spawnables.Length);
                GameObject newObject = (GameObject)Instantiate(spawnables[index], spawnPos, Quaternion.identity);
                newObject.GetComponent<Interactable>().SetWater(this);
            }
        }
        prevVelocity = velocity;

        curScale = Mathf.Lerp(curScale, targetScale, 2 * Time.deltaTime);
        prevHeight = (waterLevel+waveHeight);
        transform.parent.localScale = new Vector3(curScale, 1, transform.parent.localScale.z);
    }

    public float GetRippleTime()
    {
        return timer * rippleSpeed;
    }

    public float GetAbsoluteHeight()
    {
        return waterLevel + waveHeight;
    }

    public float GetEndPoint()
    {
        return averageHitpoint / curStep;
    }
}
