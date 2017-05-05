using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RangedEnemy : EnemyMovement 
{
	public float distance = 2f;
	public float speed = 2f;
	public int damage = 1;
	public string type = "Ranged";
	public int health;
	public float warnTimeSeconds = 1f;
	float attackRate = 1.5f;
	float nextAttack;
	GameObject player;
	public GameObject projectile;
	public float projectileSpeed;
	PlayerController playerController;
	Vector2 currentPos;
	bool death = false;

	protected void Awake () {
		base.distance = distance;
		base.speed = speed;
		base.type = type;
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
            gameObject.GetComponent<AudioSource>().Play();
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
		clone.transform.rotation = Quaternion.LookRotation(Vector3.forward, (player.transform.position - this.transform.position).normalized);
		clone.GetComponent<Rigidbody2D> (). AddForce ((playerController.getPosition()-currentPos).normalized * projectileSpeed);
	}

	public void takeDamage(int damage)
	{
		if (!death) 
		{
			health = health - damage;
			StartCoroutine (ChangeColor ());
			if (health < 1) 
			{
				StartCoroutine (DoDeath (true));	
			}
		}
	}

	public IEnumerator DoDeath(bool exp) 
	{
		death = true;
		animator.SetTrigger ("enemyDeath");
		yield return new WaitForSecondsRealtime (0.3f);
		Destroy(gameObject);
		if(SceneManager.GetActiveScene().name != "BossBattle")
			enemyManager.enemyKilled ();
		if(exp)
			playerStats.AddExperience (expToGive);
	}
}
