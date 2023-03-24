using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject monsterSpawner;
    [SerializeField] GameObject spawnedMonsters;
    [SerializeField] GameObject castleGate;
    [SerializeField] Canvas endCanvas;
    [SerializeField] TextMeshProUGUI winOrLoseText;
    [SerializeField] TextMeshProUGUI subText;
    //[SerializeField] float reloadDelay;

    private AudioSource audioSource;
    private bool gateAlive = true;
    private bool wavesComplete = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //audioSource.playOnAwake = true;
        winOrLoseText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            gateAlive = castleGate.GetComponent<Health>().IsAlive();
            wavesComplete = monsterSpawner.GetComponent<MonsterSpawner>().WavesComplete();

            if (!gateAlive)
            {
                winOrLoseText.text = "You have lost...";
                subText.text = "The monsters have broken your gate and entered the castle.";
                Debug.Log("You lose!");
                EndGame();
            }
            else if (wavesComplete)
            {
                winOrLoseText.text = "You have won!";
                subText.text = "You have defeated all waves of monsters.";
                Debug.Log("You win!");
                EndGame();
            }
        }
        else
        {
            monsterSpawner.SetActive(false);
            spawnedMonsters.SetActive(false);
            endCanvas.enabled = true;
        }
    }

    void EndGame()
    {
        monsterSpawner.SetActive(false);
        spawnedMonsters.SetActive(false);
        endCanvas.enabled = true;
    }

    public void ResetCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
