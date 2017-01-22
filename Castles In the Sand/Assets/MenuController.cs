using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class MenuController : MonoBehaviour
{
    public AudioSource music;
    public GameObject camIcon;

	// Use this for initialization
	void Start () {
        camIcon.SetActive(false);
        Time.timeScale = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Input.anyKeyDown)
        {
            Time.timeScale = 1;
            camIcon.SetActive(true);
            Camera.main.GetComponent<Blur>().enabled = false;
            music.Play();
            gameObject.SetActive(false);
        }	
	}
}
