using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyMovement 
{
	public float distance = 2f;
	public float speed = 2f;
	public int damage = 1;
	public int health;
	public float warnTimeSeconds = 1f;
	float attackRate = 2f;
	float nextAttack;
	GameObject player;
	public GameObject projectile;
	public float projectileSpeed;
	PlayerController playerController;
	Vector2 currentPos;

	protected void Awake () {
		base.distance = distance;
		base.speed = speed;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
	}

	void Update () 
	{
		currentPos = transform.position;
		base.Move ();
		if (!base.IsMoving ()) {
			attack ();
		}
		else {
			nextAttack = Time.time + attackRate;
		}        
	}

	void attack()
	{
		if(Time.time > nextAttack){
			StartCoroutine(shoot ());
			nextAttack = Time.time + attackRate;
		}
	}
	 
	IEnumerator shoot()
	{
		animator.SetBool ("enemyShoot", true);
		yield return new WaitForSecondsRealtime(warnTimeSeconds);
		animator.SetBool ("enemyShoot", false);
		GameObject clone = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		clone.transform.right = ((playerController.getPosition () - currentPos).normalized);
		clone.GetComponent<Rigidbody2D> (). AddForce ((playerController.getPosition()-currentPos).normalized * projectileSpeed);
	}

	public void takeDamage(int damage)
	{
		health = health - damage;
		if(health<1)
		{
			Destroy(gameObject);
			playerStats.AddExperience (expToGive);
		}
	}
}
