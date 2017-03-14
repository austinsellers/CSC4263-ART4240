using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float playerSpeed = .05f;
	Rigidbody2D rigidBody;
	Vector2 currentPos,projectedPos;
	bool canMove = true;
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
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		renderer = gameObject.GetComponent<SpriteRenderer> ();
		animator = gameObject.GetComponent<Animator> ();
		projectedPos = currentPos;
	}

	void Update () 
	{
		
		// Doesn't move until button is pressed
		playerMove = false;
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			// Flips the sprite to face Left
			if (renderer.flipX)
				renderer.flipX = false;
			projectedPos.x -= playerSpeed * Time.deltaTime;
			hit = Physics2D.Linecast (currentPos, projectedPos);
			if (hit.transform == null) 
			{
				currentPos.x -= playerSpeed * Time.deltaTime;
				canMove = true;
			}
			else{
				canMove = false;
			}
			// If the player is facing Left
			direction = 3;
			playerMove = true;
			//this.transform.position = position;
		}

		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			// Flips the sprite to face Right
			if (!renderer.flipX)
				renderer.flipX = true;
			
			currentPos.x += playerSpeed * Time.deltaTime;

			// If the player is facing Right
			direction = 1;
			playerMove = true;
			//this.transform.position = position;
		}

		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			currentPos.y += playerSpeed * Time.deltaTime;

			// If the player is facing Up
			direction = 0;
			playerMove = true;
			//this.transform.position = position;
		}

		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			currentPos.y -= playerSpeed * Time.deltaTime;

			// If the player is facing Down
			direction = 2;
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
		print (canMove);
		if (canMove) 
		{
			rigidBody.MovePosition (currentPos);
		}
		canMove = true;
	}
}