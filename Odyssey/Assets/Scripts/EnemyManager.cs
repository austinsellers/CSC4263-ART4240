using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour 
{
	private PlayerController player;
	public GameObject enemy;
	public float spawnTime = 5f;
	public Transform[] spawnPoints;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	void Spawn()
	{
		if (player.health <= 0) 
		{
			return;
		}

		int spawnPointIndex = Random.Range (0, spawnPoints.Length);

		Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
	}
}
