using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchBehavior : MonoBehaviour {
	float startTime;
	public float timeExisting;
	int damage = 1;
	bool didDamage = false;

	void Start ()
	{
		startTime = Time.time;
	}

	void Update ()
	{
		if ((Time.time - startTime) > timeExisting)
			DestroyImmediate(gameObject);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && !didDamage)
		{
			didDamage = true;
			if(other.gameObject.GetComponent<PlayerController>() != null)
				other.gameObject.GetComponent<PlayerController>().HurtPlayer(damage);
		}

	}

	public void SetDamage(int dmg)
	{
		this.damage = dmg;
	}
}
