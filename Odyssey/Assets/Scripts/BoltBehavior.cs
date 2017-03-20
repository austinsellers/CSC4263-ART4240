using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltBehavior : MonoBehaviour {

	float startTime;
	public float timeExisting;
	public int damage = 3;

	void Start ()
	{
		startTime = Time.time;
	}

	void Update ()
	{
		if ((Time.time - startTime) > timeExisting)
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			other.gameObject.GetComponent<PlayerController>().HurtPlayer(damage);
			Destroy(gameObject);
		}
		if (other.gameObject.tag == "Wall")
		{
			Destroy(gameObject);
		}
	}
		
}
