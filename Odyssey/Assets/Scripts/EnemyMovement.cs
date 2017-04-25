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
	Transform enemyTransform;

	protected Vector2 playerPos;	
	protected Vector2 currentPos;
	protected Vector2 localPosition;
	Vector2 projectedPos;

	BoxCollider2D boxCollider;
	Rigidbody2D rigidBody;
	protected RaycastHit2D hit;
	SpriteRenderer renderer;
	Color normalColor;

	private int dir;
	protected bool isMoving;
	public int expToGive;
	public EnemyManager enemyManager;
	public EnemyManager[] enemyManagers;
	public string enemyName;

	protected virtual void Start() 
	{
		currentPos = transform.position;
		enemyTransform = gameObject.GetComponent<Transform> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();
		animator = gameObject.GetComponent<Animator> ();
		renderer = gameObject.GetComponent<SpriteRenderer> ();
		normalColor = renderer.color;
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
			isMoving = true;

			if (type != "Melee") {
				localPosition = localPosition.normalized;
				MoveX ();
				MoveY ();
				projectedPos.Set (currentPos.x, currentPos.y);
			}
			
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

	protected IEnumerator ChangeColor() 
	{
		renderer.color = new Color (237/255.0f, 95/255.0f, 85/255.0f);
		yield return new WaitForSecondsRealtime(0.2f);
		renderer.color = normalColor;
	}

	void MoveX()
	{
		projectedPos.Set (currentPos.x + localPosition.x * speed * Time.deltaTime, currentPos.y);
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
	void MoveY ()
	{
		projectedPos.Set (currentPos.x, currentPos.y + localPosition.y * speed * Time.deltaTime);
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
