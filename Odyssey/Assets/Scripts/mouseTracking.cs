using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseTracking : MonoBehaviour {
    Vector2 mousePos;

	void Start ()
    {
       
    }
	
	void Update ()
    {
        mousePos = Input.mousePosition;
        GameObject.Find("reticle").GetComponent<Transform>().position = mousePos;
	}
}
