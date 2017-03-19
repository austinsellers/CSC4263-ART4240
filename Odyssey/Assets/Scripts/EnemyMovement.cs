using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour 
{

	Vector2 playerPos;
	protected float distance;
	Vector2 currentPos;
	Vector2 localPosition;
	Vector2 projectedPos;
	BoxCollider2D boxCollider;
	Rigidbody2D rigidBody;
	protected float speed;
	RaycastHit2D hit;
	bool isMoving;

	protected PlayerStats playerStats;
	public int expToGive;

	protected virtual void Start() 
	{
		currentPos = transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();

		playerStats = FindObjectOfType<PlayerStats> ();
	}

	protected virtual void Move () 
	{	
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
		localPosition = distanceAway ();
		if (Mathf.Abs (localPosition.y) > distance || Mathf.Abs (localPosition.x) > distance) 
		{
			isMoving = true;
			localPosition = localPosition.normalized;
			MoveX ();
			MoveY ();
			//else
			//	isMoving = false;

			//playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
			//rigidBody.MovePosition (currentPos);
			//print(distanceAway());
			projectedPos.Set(currentPos.x,currentPos.y);} 
		else 
		{
			isMoving = false;
		}
	}
	protected virtual bool IsMoving()
	{
		return isMoving;
	}
	protected virtual Vector2 distanceAway()
	{
		return playerPos - currentPos;

	}
	void MoveX()
	{
		projectedPos.Set (currentPos.x + localPosition.x * speed * Time.deltaTime, currentPos.y);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (currentPos, projectedPos);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			currentPos.Set (projectedPos.x, currentPos.y);
			rigidBody.MovePosition (currentPos);
		}  

	}
	void MoveY()
	{
		projectedPos.Set (currentPos.x, currentPos.y + localPosition.y * speed * Time.deltaTime);
		boxCollider.enabled = false;
		hit = Physics2D.Linecast (currentPos, projectedPos);
		boxCollider.enabled = true;
		if (hit.transform == null) {
			currentPos.Set (currentPos.x, projectedPos.y);
			rigidBody.MovePosition (currentPos);
		}  
	}
}