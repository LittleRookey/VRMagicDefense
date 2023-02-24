using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GateHealth : MonoBehaviour
{

    public float totalHealth = 100;
    public TextMeshProUGUI gateHealthText;

    float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = totalHealth;
        gateHealthText.SetText(currentHealth.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        currentHealth -= 10;
        gateHealthText.SetText(currentHealth.ToString());
        Debug.Log("collision detected");
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            currentHealth -= 10;
            gateHealthText.SetText(currentHealth.ToString());
            Debug.Log("collision detected");
        }
    }
    */
}
