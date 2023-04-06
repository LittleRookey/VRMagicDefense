using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject monsterSpawner;
    [SerializeField] GameObject spawnedMonsters;
    [SerializeField] GameObject castleGate;
    [SerializeField] GameObject endPanel;
    [SerializeField] Button nextLevelButton;
    [SerializeField] TextMeshProUGUI winOrLoseText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] AudioClip backgroundMusic;
    //[SerializeField] float reloadDelay;

    private AudioSource audioSource;
    private bool gateAlive = true;
    private bool wavesComplete = false;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        endPanel.SetActive(false);
        audioSource = gameObject.GetComponent<AudioSource>();

        if (backgroundMusic && audioSource)
        {
            audioSource.Play();
            audioSource.loop = true;
        }
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
                nextLevelButton.interactable = false;
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

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1) != null)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
