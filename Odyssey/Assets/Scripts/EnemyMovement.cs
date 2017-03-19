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
	Bounds NearWall;
	bool isMoving;
	protected PlayerStats playerStats;
	public int expToGive;
	bool lockY = false,lockX = false;
	protected virtual void Start() 
	{
		//NearWall = GameObject.FindGameObjectWithTag ("AIHelper").GetComponent<BoxCollider2D>().bounds;
		currentPos = transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();

		playerStats = FindObjectOfType<PlayerStats> ();
	}

	void OnTriggerEnter(Collider other) {
		print ("here");
		//Destroy(other.gameObject);
	}

	protected virtual void Move () 
	{	
		
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
		//NearWall = GameObject.FindGameObjectWithTag ("AIHelper").transform.position;
		localPosition = distanceAway ();
		//print (NearWall);
		if (Mathf.Abs (localPosition.y) > distance || Mathf.Abs (localPosition.x) > distance) 
		{
			isMoving = true;
			//print (NearWall);
			if (((currentPos.x > -5 && currentPos.x < 1) && (currentPos.y < 3 && currentPos.y > -16))) {}
				//lockX = true;
				//MoveX (0);
				//MoveY (1);
				//lockX = false;
			localPosition = localPosition.normalized;
			MoveX (1);
			MoveY (1);
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
	void MoveX(int loc)
	{
		if (!lockX) {
			projectedPos.Set (currentPos.x + localPosition.x*loc * speed * Time.deltaTime, currentPos.y);
			hit = Physics2D.Linecast (currentPos, projectedPos);
			if (hit.transform == null) {
				currentPos.Set (projectedPos.x, currentPos.y);
				rigidBody.MovePosition (currentPos);
			}  
		}
	}
	void MoveY (int loc)
	{
		if (!lockY) {
			projectedPos.Set (currentPos.x, currentPos.y + localPosition.y* loc * speed * Time.deltaTime);
			hit = Physics2D.Linecast (currentPos, projectedPos);
			if (hit.transform == null) {
				currentPos.Set (currentPos.x, projectedPos.y);
				rigidBody.MovePosition (currentPos);
			}  
		}
	}
}