using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateHealth : MonoBehaviour
{

    public float totalHealth = 100;
    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        currentHealth -= 10;
    }
}
