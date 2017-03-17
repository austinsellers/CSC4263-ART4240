using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarkBehavior : MonoBehaviour
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
}
