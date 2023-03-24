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
    [SerializeField] GameObject endPanel;
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
        endPanel.SetActive(false);
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
                subText.text = "The monsters have broken your gate and entered the castle...";
                Debug.Log("You lose!");
                EndGame();
            }
            else if (wavesComplete && spawnedMonsters.transform.childCount < 1)
            {
                winOrLoseText.text = "You have won!";
                subText.text = "You have defeated all waves of monsters.";
                Debug.Log("You win!");
                EndGame();
            }
        }
    }

    void EndGame()
    {
        monsterSpawner.SetActive(false);
        spawnedMonsters.SetActive(false);
        endPanel.SetActive(true);
    }

    public void ResetCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
