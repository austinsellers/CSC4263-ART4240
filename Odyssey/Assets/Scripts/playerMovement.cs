using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {
    Vector3 playerPos;
    

	void Start ()
    {
        
	}
	
	void Update ()
    {
        playerPos = gameObject.GetComponent<Transform>().position;
        
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            gameObject.GetComponent<Transform>().position = new Vector3(playerPos.x + (Input.GetAxis("Horizontal") / 2), playerPos.y + (Input.GetAxis("Vertical")/2), playerPos.z);
        }
    }
}
