using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField]
    private Material highlightedMat;
    [SerializeField]
    private Water water;
    [SerializeField]
    private Material submergedMat;
    [SerializeField]
    private float sinkBias;

    private Material baseMat;
    private Renderer render;

    private bool highlighted;
    private bool submerged = false;

    private float alpha = 1.0f;

	// Use this for initialization
	void Start ()
    {
        render = GetComponent<Renderer>();
        baseMat = render.material;   
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(submerged)
        {
            render.material = submergedMat;
            alpha = Mathf.Lerp(alpha, 0, Time.deltaTime*0.5f);
            Color color = render.material.GetColor("_Color");
            color.a = alpha;
            render.material.SetColor("_Color", color);
            if(alpha == 0)
            {
                Destroy(gameObject);
            }
            if (alpha < 0.05f)
            {
                alpha = 0;
            }
            transform.position = Vector3.Lerp(transform.position,new Vector3(water.GetEndPoint(), water.GetAbsoluteHeight(), transform.position.z), Time.deltaTime);
        }
        else if (highlighted)
        {
            render.material = highlightedMat;
            highlightedMat.SetTexture("_MainTex", baseMat.GetTexture("_MainTex"));
        }
        else
        {
            render.material = baseMat;
            if(transform.position.y +sinkBias < water.GetAbsoluteHeight())
            {
                submerged = true;
                return;
            }
            RaycastHit hit;
            Physics.Raycast(transform.position, -Vector3.up, out hit);
            if(hit.collider)
            {
                if(hit.collider.gameObject.tag == "Sand")
                {
                    if(transform.position.y > hit.point.y)
                    {
                        transform.position = hit.point;
                        Vector3 lookAtPoint = hit.point + Quaternion.Euler(0, 0, 90) * hit.normal;
                        transform.LookAt(lookAtPoint);
                    }
                }
            }
        }
	}

    public bool CanHighlight()
    {
        return !submerged;
    }

    public void SetHighlighted(bool value)
    {
        highlighted = value;
    }

    public void SetWater(Water newWater)
    {
        water = newWater;
    }
}
