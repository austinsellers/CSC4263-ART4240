using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyMovement : MonoBehaviour 
{

	Vector2 playerPos;
	protected float distance;
	Vector2 currentPos;
	Vector2 localPosition;
	Vector2 projectedPos;
	BoxCollider2D boxCollider;
	Rigidbody2D rigidBody;
	protected float speed;
	RaycastHit2D hit;
	bool isMoving;

	protected virtual void Start() 
	{
		currentPos = transform.position;
		rigidBody = gameObject.GetComponent<Rigidbody2D> ();
		boxCollider = gameObject.GetComponent<BoxCollider2D> ();
	}

	protected virtual void Move () 
	{	
		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
		localPosition = playerPos - currentPos;
		if (Mathf.Abs (localPosition.y) > distance || Mathf.Abs (localPosition.x) > distance) 
		{
			isMoving = true;
			localPosition = localPosition.normalized;
			projectedPos.Set (currentPos.x + localPosition.x * speed * Time.deltaTime, currentPos.y + localPosition.y * speed * Time.deltaTime);
			boxCollider.enabled = false;
			hit = Physics2D.Linecast (currentPos, projectedPos);
			boxCollider.enabled = true;
			print (hit.transform);
			if (hit.transform == null) {
				currentPos.Set (projectedPos.x, projectedPos.y);
				rigidBody.MovePosition (currentPos);
			} 
			else
				isMoving = false;
			projectedPos.Set(currentPos.x,currentPos.y);
			//playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
			//rigidBody.MovePosition (currentPos);
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
}
/*
using UnityEngine;
using System.Collections;
//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use pathfinding
using Pathfinding;

public class AstarAI : MonoBehaviour
{
  //The point to move to
  public Transform target;

  private Seeker seeker;

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
    seeker = GetComponent<Seeker>();

    //Start a new path to the targetPosition, return the result to the OnPathComplete function
    seeker.StartPath( transform.position, target.position, OnPathComplete );
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
}
*/