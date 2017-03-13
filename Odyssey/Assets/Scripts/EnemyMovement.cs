using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour 
{

	Vector2 playerPos;
	protected float distance;
	Vector2 currentPos;
	Vector2 localPosition;
	Rigidbody2D rigidBody;
	protected float speed;

	protected virtual void Start()
	{
		currentPos = transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
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
