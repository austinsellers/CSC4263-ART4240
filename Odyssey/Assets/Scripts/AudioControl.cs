using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.M))
		{
			AudioSource music = gameObject.GetComponent<AudioSource> ();
			if (music.isPlaying)
				music.Stop ();
			else
				music.Play ();
		}
	}
}
