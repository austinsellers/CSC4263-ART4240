using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyManager : MonoBehaviour 
{
	public GameObject enemy;
	public Transform[] spawnPoints;
	public float spawnTime = 5f;
	public bool pauseSpawn=false;

	void Start() 
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	public void Spawn()
	{
		if (!pauseSpawn)
		{
			int spawnPointIndex = Random.Range (0, spawnPoints.Length);
			Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
		}
	}
}
