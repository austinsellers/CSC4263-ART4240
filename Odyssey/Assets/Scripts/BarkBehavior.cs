using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkBehavior : MonoBehaviour
{
    float startTime;
    public float timeExisting;
    public int damage = 1;

    void Start()
    {
		damage = GameManager.instance.playerStats.barkDamage;
        startTime = Time.time;
    }

    void Update()
    {
        if ((Time.time - startTime) > timeExisting)
            DestroyImmediate(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<MeleeEnemy>() != null)
                other.gameObject.GetComponent<MeleeEnemy>().takeDamage(damage);
            if (other.gameObject.GetComponent<RangedEnemy>() != null)
                other.gameObject.GetComponent<RangedEnemy>().takeDamage(damage);
            Destroy(gameObject);
        }
        if (other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }

}
