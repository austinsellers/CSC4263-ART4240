using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour 
{
	private GameObject player;
	Vector3 offset;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		offset = transform.position - player.transform.position;
	}

	void Update () 
	{
		transform.position = player.transform.position + offset;	
	}
}
