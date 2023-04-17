using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffectLifetime : MonoBehaviour
{
    public float lifetime = 2;
    protected float existTime = 0;
    void Update()
    {
        existTime += Time.deltaTime;
        if (existTime >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
