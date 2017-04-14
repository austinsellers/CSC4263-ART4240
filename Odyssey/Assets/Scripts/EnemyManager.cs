using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour 
{
	private PlayerStats stats;
	public GameObject enemy;
	public string enemyName;
	public float spawnTime = 5f;
	public Transform[] spawnPoints;
	public int[] spawnAmount; //keeps track of how many enemies to spawn per wave
	private int wave=0;
	private GameManager gameManager;

	public int[] enemiesAlive; //keeps track of how many enemies are on screen
	public bool pauseSpawn=false;

	void Start () 
	{
		enemiesAlive = new int[spawnAmount.Length];
		System.Array.Copy (spawnAmount, enemiesAlive, spawnAmount.Length);
		stats = GameObject.Find("Main Canvas").GetComponent<PlayerStats>();
		if (spawnAmount.Length >= 0) 
		{
			InvokeRepeating ("Spawn", spawnTime, spawnTime);
		}
		gameManager = GameObject.FindObjectOfType<GameManager> ();

	}

	void Update() {
		wave = gameManager.wave;
	}

	void Spawn()
	{
		if (!pauseSpawn && wave < spawnAmount.Length) {
			if (spawnAmount [wave] > 0) {
				if (stats.health <= 0) {
					return;
				}

				int spawnPointIndex = Random.Range (0, spawnPoints.Length);

				Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
				spawnAmount [wave]--;
			} else {
				pauseSpawn = true;
			}
		}
	}


	IEnumerator WaitFor(float delay)
	{
		yield return new WaitForSecondsRealtime (delay);
	}

	public void enemyKilled()
	{
			enemiesAlive [wave]--;
	}
}
