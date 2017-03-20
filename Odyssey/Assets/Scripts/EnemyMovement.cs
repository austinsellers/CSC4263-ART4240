using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour 
{
	protected float distance;
	protected float speed;
	protected string type;
	protected Animator animator;
	protected PlayerStats playerStats;

	Vector2 playerPos;	
	Vector2 currentPos;
	Vector2 localPosition;
	Vector2 projectedPos;

	BoxCollider2D boxCollider;
	Rigidbody2D rigidBody;
	RaycastHit2D hit;

	private int dir;
	private bool isMoving;
	public int expToGive;
	private bool lockY = false,lockX = false;


	protected virtual void Start() 
	{
		currentPos = transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();
		animator = gameObject.GetComponent<Animator> ();

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
			MoveX (1);
			MoveY (1);
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
			projectedPos.Set (currentPos.x + localPosition.x * loc * speed * Time.deltaTime, currentPos.y);
			hit = Physics2D.Linecast (currentPos, projectedPos);
			if (hit.transform != null)
				print (hit.transform.name);
			if (!type.Equals ("Ranged")) {
				if (hit.transform == null || hit.transform.name.Equals ("Thundercloud")) {
					currentPos.Set (projectedPos.x, currentPos.y);
					rigidBody.MovePosition (currentPos);
				} 
			} else {
				if (hit.transform == null) {
					currentPos.Set (projectedPos.x, currentPos.y);
					rigidBody.MovePosition (currentPos);
				} 
			}
		}
	}
	void MoveY (int loc)
	{
		if (!lockY) {

			if (lockX) {
				if (currentPos.y >= 2 && dir == 1) {
					lockX = false;
				} else if (currentPos.y <= -15 && dir == -1) {
					lockX = false;
				}
				currentPos.Set (currentPos.x, currentPos.y + dir * speed * Time.deltaTime);
				rigidBody.MovePosition (currentPos);
			} else {
				projectedPos.Set (currentPos.x, currentPos.y + localPosition.y * loc * speed * Time.deltaTime);
				hit = Physics2D.Linecast (currentPos, projectedPos);
				if (!type.Equals ("Ranged")) {
					if (hit.transform == null || hit.transform.name.Equals ("Thundercloud")) {
						currentPos.Set (currentPos.x, projectedPos.y);
						rigidBody.MovePosition (currentPos);
					}  
				}else {
					if (hit.transform != null)
						print (hit.transform.name);
					if (hit.transform == null) {
						currentPos.Set (currentPos.x, projectedPos.y);
						rigidBody.MovePosition (currentPos);
					} 
				}
			}
		}
	}
}