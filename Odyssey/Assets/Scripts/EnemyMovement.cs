using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour 
{

	Vector2 playerPos;
	float distance;
	Vector2 currentPos;
	Vector2 localPosition;
	Rigidbody2D rigidBody;
	float speed;
	protected virtual void Start(string name)
	{
		if (name == "Melee") 
		{
			distance = 2f;
			speed = 2f;
			currentPos = transform.position;
			rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		}
		if (name == "Ranged") 
		{
			distance = 4.5f;
			speed = 2f;
			currentPos = transform.position;
			rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		}
	}

	protected virtual void Move () 
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
