using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 10f;   // this is the projectile's speed
    public float lifespan = 5f; // this is the projectile's lifespan (in seconds)

    private Rigidbody rb;
    private Vector3 direction;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        //transform.Translate(direction * Time.deltaTime);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
