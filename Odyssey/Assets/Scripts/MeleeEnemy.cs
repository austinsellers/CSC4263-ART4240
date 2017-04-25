using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyMovement 
{
	public GameObject Scratch;

	public float distance = 2f;
	public float speed = 2f;
	public int damage = 1;
	public string type = "Melee";
    public int health;
	public float attackRate = 3f;
	public Vector2 biteScale = new Vector2(1f,1f);
	public float distanceFromAtk = 1.5f;
	float nextAttack;
	GameObject player;
	PlayerController playerController;
	AStar catAStar;

	protected void Awake () 
	{
		base.distance = distance;
		base.speed = speed;
		base.type = type;
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		if (enemyName.Equals ("Cat") || enemyName.Equals ("Cat(Clone)")) 
		{
			catAStar = gameObject.GetComponent<AStar> ();
		}
	}

	// Update is called once per frame
	protected void Update () 
	{
		base.Move();
		//print ("here");
		if (!base.IsMoving()) 
		{
		//	attack ();
		}
		else 
		{
			nextAttack = Time.time + attackRate;
		}        
	}

	protected void attack() 
	{
		print ("attacking");
		if(Time.time > nextAttack)
		{
			
				gameObject.GetComponent<AudioSource> ().Play ();
			  MakeScratch (player.gameObject.transform.position);
				playerController.HurtPlayer (damage);
			nextAttack = Time.time + attackRate;
		}
	}

	void MakeScratch(Vector3 dir)
	{
		GameObject scratch;
		Vector2 currentPos = gameObject.transform.position;
		scratch = (GameObject)Instantiate(Scratch, dir, //new Vector3(currentPos.x + distanceFromAtk * Mathf.Sin(dir * Mathf.PI / 2f), currentPos.y + distanceFromAtk * Mathf.Cos(dir * Mathf.PI / 2f), -5f), 
			new Quaternion(0f, 0f, 0f ,0f));
		//scratch.GetComponent<Transform>().localScale = new Vector3(biteScale.x, biteScale.y, 0f);
		scratch.transform.parent = gameObject.transform;
		//scratch.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, -90f));
		scratch.GetComponent<ScratchBehavior> ().SetDamage(damage);
	}

    public void takeDamage(int damage)
    {
        health = health - damage;
		StartCoroutine (ChangeColor ());
        if(health<1)
        {
            Destroy(gameObject);
			enemyManager.enemyKilled ();
			playerStats.AddExperience (expToGive);
        }
    }
}
