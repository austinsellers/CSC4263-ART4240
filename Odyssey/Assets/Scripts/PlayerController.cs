using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public GameObject Bite;

	public float speed = 6f;
	float movement;

	public LayerMask blockingLayer;
	Rigidbody2D rigidBody;
	Vector2 currentPos,projectedPos;
	BoxCollider2D boxCollider;
	RaycastHit2D hit;
	SpriteRenderer renderer;
	Animator animator;

	/*
	 * Starts out facing LEFT
	 * Directions are like this:
	 * 			0
	 * 		3		1
	 * 			2
	 */
	private int direction = 3;
	private bool playerMove = false;

	void Start () 
	{
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		renderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();

	}

	void Update () 
	{
		// Doesn't move until button is pressed
		playerMove = false;
		//projectedPos.Set(currentPos.x,currentPos.y);
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			// Flips the sprite to face Left
			if (renderer.flipX)
				renderer.flipX = false;
			// If the player is facing Left
			direction = 3;
			Move (direction);
			playerMove = true;
		}

		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			// Flips the sprite to face Right
			if (!renderer.flipX)
				renderer.flipX = true;
			// If the player is facing Right
			direction = 1;
			Move (direction);
			playerMove = true;
			//this.transform.position = position;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{

			// If the player is facing Up
			direction = 0;
			Move (direction);
			playerMove = true;
			//this.transform.position = position;
		}

		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			// If the player is facing Down
			direction = 2;
			Move (direction);
			playerMove = true;
			//this.transform.position = position;
		}

		// If player is going left or right set that trigger (For Animation)
		if (direction == 1 || direction == 3) 
		{
			animator.SetBool ("playerLR", true);
			animator.SetBool ("playerUD", false);
		} 
		else 
		{
			animator.SetBool ("playerLR", false);
			animator.SetBool ("playerUD", true);
		}
		// Sets if the player is moving (For Animation)
		animator.SetBool ("playerMove", playerMove);
		animator.SetInteger ("playerDir", direction);
		if (Input.GetMouseButtonDown(0))
		{
			BiteMake(direction);
		}
		print (projectedPos + " " + currentPos);
		projectedPos.Set(currentPos.x,currentPos.y);
	}

	void Move(int dir)
	{
		movement = speed * Time.deltaTime;
		if (dir == 3)
			projectedPos += Vector2.left * movement;
		if (dir == 1)
			projectedPos += Vector2.right * movement;
		if (dir == 0)
			projectedPos += Vector2.up * movement;
		if (dir == 2)
			projectedPos += Vector2.down * movement;

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (new Vector2(currentPos.x,currentPos.y), projectedPos, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null ) 
		{
			currentPos.Set(projectedPos.x,projectedPos.y);
			rigidBody.MovePosition (currentPos);
		}
	}

	void BiteMake(int dir)
	{
		GameObject bite;
		bite = (GameObject)Instantiate(Bite, currentPos, Quaternion.identity);

	}
}