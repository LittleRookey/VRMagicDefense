using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Monster parentMonster;
    public float attackDmg = 5f;
    public float speed = 10f;   // this is the projectile's speed
    public float lifespan = 2f; // this is the projectile's lifespan (in seconds)
    Transform target;

    private Vector3 direction;

    void Awake()
    {
        parentMonster = gameObject.GetComponentInParent<Monster>();
        if (parentMonster)
        {
            target = parentMonster.GetTarget();
        }

        if (target)
        {
            direction = (target.position - transform.position).normalized;
        }
        else
        {
            direction = transform.forward;
        }
    }

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Castle"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log("HIT");
                health.TakeDamage(attackDmg);
            }
        }
    }
}
