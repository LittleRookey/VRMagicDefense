using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject monsterSpawner;
    [SerializeField] GameObject castleGate;
    [SerializeField] Canvas endCanvas;
    [SerializeField] TextMeshProUGUI winOrLoseText;
    [SerializeField] TextMeshProUGUI subText;
    //[SerializeField] float reloadDelay;

    private AudioSource audioSource;
    private float gateHealth;
    private bool wavesComplete;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //audioSource.playOnAwake = true;
        gateHealth = castleGate.GetComponent<Health>().GetCurrentHealth();
        winOrLoseText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        gateHealth = castleGate.GetComponent<Health>().GetCurrentHealth();
        wavesComplete = monsterSpawner.GetComponent<MonsterSpawner>().WavesComplete();

        if (gateHealth <= 0.0)
        {
            EndGame(false);
            Debug.Log("You lose!");
        }
        else if (wavesComplete && gateHealth > 0.0)
        {
            EndGame(true);
            Debug.Log("You win!");
        }
    }

    void EndGame(bool gameWon)
    {
        Time.timeScale = 0;
        monsterSpawner.SetActive(false);

        if (gameWon)
        {
            winOrLoseText.text = "You have won!";
            subText.text = "You have defeated all waves of monsters.";
        }
        else
        {
            winOrLoseText.text = "You have lost...";
            subText.text = "The monsters have broken your gate and entered the castle.";
        }

        winOrLoseText.enabled = true;

        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // end game win versus end game lose
    }
}
