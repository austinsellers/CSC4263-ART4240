using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float speed = .05f;
	Rigidbody2D rigidBody;
	Vector2 currentPos;
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
		animator = GetComponent<Animator> ();

	}

	void Update () 
	{
		playerMove = false;
		/*currentPos.x = Input.GetAxis ("Horizontal");
		currentPos.y = Input.GetAxis ("Vertical");
		rigidBody.velocity = Vector3.ClampMagnitude (currentPos, 1) * speed;*/
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			// Flips the sprite to face Left
			if (renderer.flipX)
				renderer.flipX = false;
			
			currentPos.x -= speed * Time.deltaTime;

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
			
			currentPos.x += speed * Time.deltaTime;

			// If the player is facing Right
			direction = 1;
			playerMove = true;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			currentPos.y += speed * Time.deltaTime;

			// If the player is facing Up
			direction = 0;
			playerMove = true;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			currentPos.y -= speed * Time.deltaTime;

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
		animator.SetBool ("playerMove", playerMove);

		rigidBody.MovePosition (currentPos);
	}
}