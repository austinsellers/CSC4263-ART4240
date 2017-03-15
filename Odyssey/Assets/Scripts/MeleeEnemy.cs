using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyMovement 
{
	public float distance = 2f;
	public float speed = 2f;
	public int damage = 1;
	float attackRate = 3f;
	float nextAttack;
	GameObject player;
	PlayerController playerController;

	protected void Awake () {
		base.distance = distance;
		base.speed = speed;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = (PlayerController)player.GetComponent (typeof(PlayerController));
	}

	// Update is called once per frame
	void Update () {
		base.Move();
		if (!base.IsMoving ()) {
			attack ();
		}
		else {
			nextAttack = Time.time + attackRate;
		}

	}

	void attack() {
		if(Time.time > nextAttack){
			playerController.HurtPlayer (damage);
			nextAttack = Time.time + attackRate;
		}
	}
}
