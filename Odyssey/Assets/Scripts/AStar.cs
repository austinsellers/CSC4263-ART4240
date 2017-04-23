using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class AStar: MonoBehaviour
{
	//The point to move to
	public Transform target;
	private Seeker seeker;
	private PlayerController player;
	//The calculated path
	public Path path;
	//The AI's speed per second
	public float speed = 2;
	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	Vector3 rotat = new Vector3 (0f, 0f, 0f);
	float offset = 3.5f;
	[HideInInspector]
	public int dir;
	Animator animator;
	SpriteRenderer renderer;

	public void Start ()
	{
		AstarPath.active.Scan();
		seeker = GetComponent<Seeker>();
		animator = GetComponent<Animator> ();
		renderer = GetComponent<SpriteRenderer> ();
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		player =  GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		StartCoroutine(MakeNewPath());
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		//seeker.StartPath( transform.position, target.position, OnPathComplete );
	}
	public void OnPathComplete ( Path p )
	{
		Debug.Log( "Yay, we got a path back. Did it have an error? " + p.error );
		if (!p.error)
		{
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
		}
	}

	void Rotate(float angle) 
	{
		rotat.z = angle;
		gameObject.transform.rotation = Quaternion.Euler (rotat);
	}

	void Update()
	{
		bool xArea = (transform.position.x < target.position.x+offset && transform.position.x > target.position.x-offset);
		bool yArea = (transform.position.y < target.position.y+offset && transform.position.y > target.position.y-offset);

		if ((xArea && transform.position.y < target.position.y) && (yArea && transform.position.x >= target.position.x))
		{
			dir = 0; // Move UP, LEFT
			Rotate(30f);
		}
		else if ((xArea && transform.position.y < target.position.y) && (yArea && transform.position.x < target.position.x))
		{
			dir = 0; // Move UP, RIGHT
			Rotate(-30f);
		}
		else if ((xArea && transform.position.y >= target.position.y) && (yArea && transform.position.x < target.position.x))
		{
			dir = 2; // Move DOWN, RIGHT
			Rotate(30f);
		}
		else if ((xArea && transform.position.y >= target.position.y) && (yArea && transform.position.x >= target.position.x))
		{
			dir = 2; // Move DOWN, LEFT
			Rotate(-30f);
		}
		else if (yArea && transform.position.x >= target.position.x)
		{
			// Flips the sprite to face Left
			if (renderer.flipX)
				renderer.flipX = false;
			// If the enemy is facing Left
			dir = 3;
			// Reset Rotation
			Rotate(0f);
		}
		else if (xArea && transform.position.y < target.position.y)
		{
			// If the enemy is facing Up
			dir = 0;
			// Reset Rotation
			Rotate(0f);
		}
		else if (xArea && transform.position.y >= target.position.y)
		{
			// If the enemy is facing Down
			dir = 2;
			// Reset Rotation
			Rotate(0f);
		}
		else if (yArea && transform.position.x < target.position.x)
		{
			// Flips the sprite to face Right
			if (!renderer.flipX)
				renderer.flipX = true;
			// If the enemy is facing Right
			dir = 1;
			// Reset Rotation
			Rotate(0f);
		}
		if (dir == 1 || dir == 3)
			animator.SetBool("enemyLR", true);
		else
			animator.SetBool("enemyLR", false);
		animator.SetBool ("enemyMove", true);
		animator.SetInteger ("enemyDir", dir);
	}

	public void FixedUpdate ()
	{
		
	    //seeker.StartPath( transform.position, target.position, OnPathComplete );
		//print (player.didPlayerMove ());
		if (path == null)
		{
			//We have no path to move after yet
			return;
		}

		if (currentWaypoint >= path.vectorPath.Count)
		{
			Debug.Log( "End Of Path Reached" );
			return;
		}

		//Direction to the next waypoint
		Vector3 dir = ( path.vectorPath[ currentWaypoint ] - transform.position ).normalized;
		dir *= speed * Time.fixedDeltaTime;
		this.gameObject.transform.Translate( dir );

		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance( transform.position, path.vectorPath[ currentWaypoint ] ) < nextWaypointDistance)
		{
			currentWaypoint++;
			return;
		}
	}
	IEnumerator MakeNewPath()
	{
		seeker.StartPath(transform.position,target.position,OnPathComplete );
		yield return new WaitForSeconds (1);
		StartCoroutine(MakeNewPath ());
	}

}