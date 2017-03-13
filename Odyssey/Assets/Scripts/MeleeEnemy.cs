using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyMovement 
{
	public float distance = 2f;
	public float speed = 2f;

	protected void Awake () {
		base.distance = distance;
		base.speed = speed;
	}
	
	// Update is called once per frame
	void Update () {
		base.Move();
	}
}
