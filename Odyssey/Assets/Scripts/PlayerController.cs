using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	public float speed = .05f;
	Rigidbody2D rigidBody;
	Vector2 currentPos;
	Vector2 initPos;
	void Start () 
	{
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		initPos = gameObject.transform.position;

	}
	void Update () {
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			currentPos.x -= speed*Time.deltaTime;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			currentPos.x += speed*Time.deltaTime;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			currentPos.y+=speed*Time.deltaTime ;
			//this.transform.position = position;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			currentPos.y-=speed*Time.deltaTime;
			//this.transform.position = position;
		}
		rigidBody.MovePosition (currentPos);
	}
}