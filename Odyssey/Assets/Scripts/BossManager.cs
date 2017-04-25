using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour 
{
	public GameObject enemy;
	public Transform[] spawnPoints;
	public float spawnTime = 5f;

	void Start() 
	{
		InvokeRepeating ("Spawn", spawnTime, spawnTime);
	}

	public void Spawn()
	{
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		Instantiate (enemy, spawnPoints [spawnPointIndex].position, spawnPoints [spawnPointIndex].rotation);
	}
}
