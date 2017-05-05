using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossEnemy : EnemyMovement 
{
	public GameObject Pole;

	public float distance = 2f;
	public int shootAmount = 3;
	public float speed = 2f;
	public int damage = 1;
	public string type = "Boss";
	public int health;
	public float warnTimeSeconds = 1f;
	float attackRate = 1f;
	float attackPoleRate = 1f;
	float nextAttack;
	float nextPoleAttack;
	GameObject player;

	public GameObject projectile;
	public float projectileSpeed;

	public Vector2 poleScale = new Vector2(1f,1f);
	public float distanceFromAtk = 1.5f;
	PlayerController playerController;

	public Transform[] wayPoints;
	public bool isCircular;
	// Always true at the beginning because the moving object will always move towards the first waypoint
	public bool inReverse = true;

	private Transform currentWaypoint;
	private int currentIndex = 0;
	private bool isWaiting = false;
	private Transform transform;

	bool poleAttack = false;
	bool death = false;

	public BossEnemyManager[] bossEnemyManagers;

	[HideInInspector]
	public float oldSpeed;

	protected void Awake () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		playerController = player.GetComponent<PlayerController>();
		bossEnemyManagers = GameObject.FindObjectsOfType<BossEnemyManager> ();
	}

	void Start () 
	{
		base.Start ();
		if (wayPoints.Length > 0) 
		{
			currentWaypoint = wayPoints [0];
		}
		transform = gameObject.GetComponent<Transform> ();
	}

	// Update is called once per frame
	protected void Update () 
	{
		currentPos = (Vector2) gameObject.transform.position;
		if (poleAttack)
		{
			DoPoleAttack ();
			StartCoroutine (WaitFor(0.2f, true));
		}
		else if (currentWaypoint != null && !isWaiting) 
		{
			MoveTowardsWaypoint ();
		}

		else if (isWaiting)
		{
			attack ();
			StartCoroutine (WaitFor(1f, false));
		}
		else 
		{
			nextAttack = Time.time + attackRate;
		} 
		animator.SetBool ("bossWalk", !isWaiting);
	}

	void PauseBoss()
	{
		isWaiting = !isWaiting;
	}

	private void MoveTowardsWaypoint() 
	{
		Vector3 currentPosition = this.transform.position;

		Vector3 targetPosition = currentWaypoint.transform.position;

		playerPos = GameObject.FindGameObjectWithTag ("Player").transform.position;	
		localPosition = distanceAway ();
		if (Mathf.Sqrt(Mathf.Pow(localPosition.x,2)+Mathf.Pow(localPosition.y,2)) < distance && !poleAttack) 
		{
			poleAttack = true;
		} 
		else 
		{
			poleAttack = false;
			if (Vector3.Distance (currentPosition, targetPosition) > .1f) {
				Vector3 directionOfTravel = targetPosition - currentPosition;
				directionOfTravel.Normalize ();
				if (directionOfTravel.x <= 0.0)
					transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				if (directionOfTravel.x > 0.0)
					transform.localScale = new Vector3 (-1.0f, 1.0f, 1.0f);
				directionOfTravel.Set (directionOfTravel.x * speed * Time.deltaTime,
					directionOfTravel.y * speed * Time.deltaTime,
					directionOfTravel.z * speed * Time.deltaTime);

				this.transform.Translate (
					directionOfTravel,
					Space.World
				);
			} else {
				PauseBoss ();
				NextWaypoint ();
			}
		}
	}

	/*void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("Player")) 
		{
			PauseBoss ();
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("Player")) 
		{
			attack();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.name.Equals ("Player")) 
		{
			PauseBoss ();
		}
	}*/

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

	protected void attack() 
	{
		if(Time.time > nextAttack)
		{
			animator.SetTrigger ("bossNet");
			animator.SetBool ("bossNet2", true);
			StartCoroutine (shoot ());

			nextAttack = Time.time + attackRate;
		}
	}

	private void DoPoleAttack()
	{
		Debug.Log ("ATTACK!");
		if (Time.time > nextPoleAttack) 
		{
			animator.SetTrigger ("bossPole");
			animator.SetBool ("bossPole2", true);

			playerController.HurtPlayer (damage);
			
			nextPoleAttack = Time.time + attackPoleRate;

		}
		poleAttack = false;
	}

	IEnumerator shoot()
	{
		//animator.SetBool ("enemyShoot", true);
		yield return new WaitForSecondsRealtime(warnTimeSeconds);
		//animator.SetBool ("enemyShoot", false);
		GameObject clone = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		clone.transform.rotation = Quaternion.LookRotation(Vector3.forward, -(player.transform.position - this.transform.position).normalized);
		clone.GetComponent<Rigidbody2D> (). AddForce ((playerController.getPosition()-(Vector2)gameObject.transform.position).normalized * projectileSpeed);
		//clone.GetComponent<Rigidbody2D> (). AddForce ((playerController.getPosition()-currentPos).normalized * projectileSpeed);
		yield return new WaitForSecondsRealtime(0.5f);
		GameObject clone2 = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		clone2.transform.rotation = Quaternion.LookRotation(Vector3.forward, -Vector3.down);
		clone2.GetComponent<Rigidbody2D> (). AddForce (Vector2.down * projectileSpeed);
		PauseBoss();
	}

	void MakePole(int dir)
	{
		GameObject pole;
		Vector2 currentPos = gameObject.transform.position;
		pole = (GameObject)Instantiate(Pole, new Vector3(currentPos.x + distanceFromAtk * Mathf.Sin(dir * Mathf.PI / 2f), currentPos.y + distanceFromAtk * Mathf.Cos(dir * Mathf.PI / 2f), -5f), new Quaternion(0f, 0f, 0f ,0f));
		pole.GetComponent<Transform>().localScale = new Vector3(poleScale.x, poleScale.y, 0f);
		pole.transform.parent = gameObject.transform;
		pole.GetComponent<Transform>().Rotate(new Vector3(0f, 0f, dir * -90f));
		pole.GetComponent<ScratchBehavior> ().SetDamage(damage);
	}

	public void takeDamage(int damage)
	{
		if (!death) 
		{
			health = health - damage;
			StartCoroutine (ChangeColor ());
			if (health < 1) 
			{
				StartCoroutine (DoDeath());
			}
		}
	}

	IEnumerator DoDeath() 
	{
		death = true;
		pauseSpawning (true);
		DeleteEnemies ();
		animator.SetTrigger ("bossDeath");
		yield return new WaitForSecondsRealtime (2f);
		// playerStats.AddExperience (expToGive);
		GameManager.instance.Win ();
	}

	void pauseSpawning(bool pauseSpawn)
	{
		for (int i = 0; i < bossEnemyManagers.Length; i++) 
		{
			bossEnemyManagers [i].pauseSpawn = pauseSpawn;
		}
	}

	private void DeleteEnemies()
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) 
		{
			if (g.gameObject.GetComponent<MeleeEnemy> () != null)
				StartCoroutine (g.gameObject.GetComponent<MeleeEnemy> ().DoDeath (false));
			if (g.gameObject.GetComponent<RangedEnemy> () != null)
				StartCoroutine(g.gameObject.GetComponent<RangedEnemy> ().DoDeath (false));
		}
	}

	IEnumerator WaitFor(float delay, bool pole)
	{
		yield return new WaitForSecondsRealtime (delay);
		if(pole)
			animator.SetBool ("bossPole2", false);
		else
			animator.SetBool ("bossNet2", false);
	}
}
