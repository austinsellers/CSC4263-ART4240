using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{
	public float speed = .05f;
	Rigidbody2D rigidBody;
	Vector2 currentPos;
	Vector2 initPos;

	void Start () 
	{
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		initPos = gameObject.transform.position;

	}
	void Update () 
	{
		/*currentPos.x = Input.GetAxis ("Horizontal");
		currentPos.y = Input.GetAxis ("Vertical");
		rigidBody.velocity = Vector3.ClampMagnitude (currentPos, 1) * speed;*/
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
		{
			currentPos.x -= speed * Time.deltaTime;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
		{
			currentPos.x += speed * Time.deltaTime;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
		{
			currentPos.y += speed * Time.deltaTime;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
		{
			currentPos.y -= speed * Time.deltaTime;
			//this.transform.position = position;
		}
		rigidBody.MovePosition (currentPos);
	}
}