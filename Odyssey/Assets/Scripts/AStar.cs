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
	public void Start ()
	{
		AstarPath.active.Scan();
		seeker = GetComponent<Seeker>();
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