using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour 
{
	private PlayerStats stats;
	public GameObject enemy;
	public float spawnTime = 5f;
	public Transform[] spawnPoints;

	void Start () 
	{
		stats = GameObject.Find("Main Canvas").GetComponent<PlayerStats>();
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	void Spawn()
	{
		if (stats.health <= 0) 
		{
			return;
		}

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
	}
}
