using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteBehavior : MonoBehaviour {
    float startTime;
    public float timeExisting;
    public int damage = 3;
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
        if (other.gameObject.tag == "Enemy" && !didDamage)
        {
            didDamage = false;
            other.gameObject.GetComponent<MeleeEnemy>().takeDamage(damage);
        }
    }
}
