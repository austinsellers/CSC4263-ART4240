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
            other.gameObject.GetComponent<MeleeEnemy>().takeDamage(damage);
            Destroy(gameObject);
        } 
    }

}
