using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    //public Monster parentMonster;
    public Vector3 direction;
    public float attackDmg = 5f;
    public float speed = 10f;   // this is the projectile's speed
    public float lifespan = 2f; // this is the projectile's lifespan (in seconds)
    Transform target;

    Rigidbody rb;
    [SerializeField] private GameObject hitEffect;

    //private Vector3 direction;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(gameObject, lifespan);
    }

    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(rb.velocity);
        if (direction != null)
        {
            //transform.forward = direction;
            transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Castle"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null)
            {
                Debug.Log("HIT " + health.gameObject.name);
                health.TakeDamage(attackDmg);
            }
            var hitEff = Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
            Destroy(gameObject, 0.1f);
        }
    }
}
