using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyMovement 
{
	public float distance = 2f;
	public float speed = 2f;
	public int damage = 1;
	public string type = "Melee";
    public int health;
	public float attackRate = 3f;
	float nextAttack;
	GameObject player;
	PlayerController playerController;

	protected void Awake () {
		base.distance = distance;
		base.speed = speed;
		base.type = type;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
	}

	// Update is called once per frame
	protected void Update () {
		base.Move();
		if (!base.IsMoving ()) {
			attack ();
		}
		else {
			nextAttack = Time.time + attackRate;
		}        
	}

	protected void attack() {
		if(Time.time > nextAttack){
			playerController.HurtPlayer (damage);
			nextAttack = Time.time + attackRate;
		}
	}

    public void takeDamage(int damage)
    {
        health = health - damage;
        if(health<1)
        {
            Destroy(gameObject);
			enemyManager.enemyKilled ();
			playerStats.AddExperience (expToGive);
        }
    }
}
