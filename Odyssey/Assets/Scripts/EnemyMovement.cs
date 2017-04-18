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
	protected RaycastHit2D hit;
	SpriteRenderer renderer;

	private int dir;
	protected bool isMoving;
	public int expToGive;
	private bool lockY = false,lockX = false;
	public EnemyManager enemyManager;
	public EnemyManager[] enemyManagers;
	public string enemyName;

	protected virtual void Start() 
	{
		currentPos = transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();
		animator = gameObject.GetComponent<Animator> ();
		renderer = gameObject.GetComponent<SpriteRenderer> ();
		playerStats = FindObjectOfType<PlayerStats> ();
			enemyManagers = GameObject.FindObjectsOfType<EnemyManager> ();

		for (int i = 0; i < enemyManagers.Length; i++) 
		{
			if (enemyManagers [i].enemyName.Equals(this.enemyName)) {
				enemyManager=enemyManagers[i];
			}
		}
	}
		

	protected virtual void Move () 
	{	
		
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
		localPosition = distanceAway ();
		if (Mathf.Abs (localPosition.y) > distance || Mathf.Abs (localPosition.x) > distance) 
		{
			if (localPosition.x >= 0) 
			{
				renderer.flipX = true;
			}
			if (localPosition.x < 0)
			{
				renderer.flipX = false;
			}
			isMoving = true;
			localPosition = localPosition.normalized;
			MoveX (1);
			MoveY (1);
			projectedPos.Set(currentPos.x,currentPos.y);
		} 
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
		if (!lockX)
		{
			projectedPos.Set (currentPos.x + localPosition.x * loc * speed * Time.deltaTime, currentPos.y);
			hit = Physics2D.Linecast (currentPos, projectedPos);
			if (!type.Equals ("Ranged"))
			{
				if (hit.transform == null || hit.transform.name.Equals ("Thundercloud") || hit.transform.name.Equals("Thundercloud(Clone)"))
				{
					currentPos.Set (projectedPos.x, currentPos.y);
					rigidBody.MovePosition (currentPos);
				} 
			} 
			else 
			{
				if (hit.transform == null || hit.transform.tag.Equals ("Wall")) 
				{
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
					if (hit.transform == null || hit.transform.name.Equals ("Thundercloud") || hit.transform.name.Equals("Thundercloud(Clone)"))
					{
						currentPos.Set (currentPos.x, projectedPos.y);
						rigidBody.MovePosition (currentPos);
					}  
				} 
				else 
				{
					if (hit.transform == null || hit.transform.tag.Equals ("Wall")) {
						currentPos.Set (currentPos.x, projectedPos.y);
						rigidBody.MovePosition (currentPos);
					} 
				}
			}
		}
	}
}
