using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMovement : MonoBehaviour 
{

	Vector2 playerPos;
	float distance;
	Vector2 currentPos;
	Vector2 localPosition;
	Rigidbody2D rigidBody;
	float speed = 2f;
	void Start ()
	{
		distance = 2f;
		playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		currentPos = transform.position;
	}

	void Update () 
	{
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
		localPosition = playerPos - currentPos;
		if ( Mathf.Abs(localPosition.y) > distance || Mathf.Abs(localPosition.x) > distance) 
		{
			localPosition = localPosition.normalized;
			currentPos.Set (currentPos.x += localPosition.x * speed * Time.deltaTime, currentPos.y += localPosition.y * speed * Time.deltaTime);
			playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
			rigidBody.MovePosition (currentPos);
		}
	}
}
