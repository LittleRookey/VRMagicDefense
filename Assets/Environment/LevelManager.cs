using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Health playerHealth;
    [SerializeField] GameObject monsterSpawner;
    [SerializeField] GameObject spawnedMonsters;
    [SerializeField] GameObject target;
    [SerializeField] GameObject endPanel;
    [SerializeField] GameObject nextLevelButton;
    [SerializeField] TextMeshProUGUI winOrLoseText;
    [SerializeField] TextMeshProUGUI subText;
    [SerializeField] AudioClip backgroundMusic;
    //[SerializeField] float reloadDelay;

    private AudioSource audioSource;
    private bool targetAlive = true;
    private bool wavesComplete = false;
    private bool gameOver = false;
    private SpellCaster player;

    // Start is called before the first frame update
    void Start()
    {
        //DontDestroyOnLoad(gameObject);

        endPanel.SetActive(false);
        player = GameObject.FindObjectOfType<SpellCaster>();
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
        if (!gameOver && target != null)
        {
            targetAlive = target.GetComponent<Health>().IsAlive();
            wavesComplete = monsterSpawner.GetComponent<MonsterSpawner>().WavesComplete();

            if (!targetAlive || (playerHealth != null && !playerHealth.IsAlive()))
            {
                winOrLoseText.text = "You have lost...";
                subText.text = "The monsters have defeated you...";
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
        gameOver = true;
        monsterSpawner.SetActive(false);
        spawnedMonsters.SetActive(false);
        endPanel.SetActive(true);
        //if (SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).IsValid())
        //{
        //Debug.Log("TRUE");
        //nextLevelButton.SetActive(true);
        //}
    }

    public void ResetCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        player.Save(nextSceneIndex);
        SceneManager.LoadScene(nextSceneIndex, LoadSceneMode.Single);
    }
}
