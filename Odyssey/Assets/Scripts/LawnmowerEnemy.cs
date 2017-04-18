using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LawnmowerEnemy : MeleeEnemy 
{
	public Transform[] wayPoints;
	public bool isCircular;
	// Always true at the beginning because the moving object will always move towards the first waypoint
	public bool inReverse = true;

	private Transform currentWaypoint;
	private int currentIndex = 0;
	private bool isWaiting = false;
	private Transform transform;

	void Start () 
	{
		if (wayPoints.Length > 0) 
		{
			currentWaypoint = wayPoints [0];
		}
		transform = gameObject.GetComponent<Transform> ();
	}

	void Update () 
	{
		if (currentWaypoint != null && !isWaiting) 
		{
			MoveTowardsWaypoint ();
		}
	}

	void PauseMower()
	{
		isWaiting = !isWaiting;
	}

	private void MoveTowardsWaypoint() 
	{
		Vector3 currentPosition = this.transform.position;

		Vector3 targetPosition = currentWaypoint.transform.position;

		if (Vector3.Distance (currentPosition, targetPosition) > .1f) {
			Vector3 directionOfTravel = targetPosition - currentPosition;
			directionOfTravel.Normalize ();
			if (directionOfTravel.x == 1.0)
				transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
			if (directionOfTravel.x == -1.0)
				transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
			directionOfTravel.Set (directionOfTravel.x * speed * Time.deltaTime,
				directionOfTravel.y * speed * Time.deltaTime,
				directionOfTravel.z * speed * Time.deltaTime);

			this.transform.Translate (
				directionOfTravel,
				Space.World
			);
		} else {
			NextWaypoint ();
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("Player")) 
		{
			PauseMower ();
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("Player")) 
		{
			base.attack ();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("Player")) 
		{
			PauseMower ();
		}
	}

	private void NextWaypoint()
	{
		if (isCircular) 
		{
			if (!inReverse) 
			{
				if (currentIndex + 1 >= wayPoints.Length)
					currentIndex = 0;
				else
					currentIndex++;
			} 
			else 
			{
				if (currentIndex == 0)
					currentIndex = wayPoints.Length - 1;
				else
					currentIndex--;
			}
		}
		else 
		{
			// If at start or the end then reverse
			if((!inReverse && (currentIndex + 1 >= wayPoints.Length)) || (inReverse && currentIndex == 0)) 
			{
				inReverse = !inReverse;	
			}

			if (!inReverse)
				currentIndex++;
			else
				currentIndex--;
		}
		currentWaypoint = wayPoints [currentIndex];
	}
}
