using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetBehavior : MonoBehaviour
{
	float startTime;
	public float timeExisting;

	void Start()
	{
		startTime = Time.time;
	}

	void Update()
	{

		if ((Time.time - startTime) > timeExisting)
			DestroyImmediate(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerController>().SlowdownPlayer(3.0f);
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "Wall")
		{
			Destroy(gameObject);
		}
	}

}
