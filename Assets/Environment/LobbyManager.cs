using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public enum TutorialState
{
    Welcome,
    LearnGrabBook,
    LearnMoving,
    LearnFlipPage,
    LearnSpells,
    LearnCastSpell,
    LearnNewSpell,
    Boss,
    StartGame
}
public class LobbyManager : MonoBehaviour
{
    public GameObject startGame;
    public GameObject tutorial;

    public GameObject dummy1;
    public GameObject dummy2;
    public AudioClip backgroundMusic;
    private AudioSource audioSource;
    private SpellCaster player;
    private bool canNext = true;

    private TutorialState tutorialState;
    void Start()
    {
        tutorialState = TutorialState.Welcome;
        audioSource = gameObject.GetComponent<AudioSource>();
        player = GameObject.FindObjectOfType<SpellCaster>();

        if (backgroundMusic && audioSource)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
            audioSource.loop = true;
        }

        int currentLevel = player.GetCurrentLevelProgression();

        if (currentLevel == 0)
        {
            startGame.transform.Find("SubText").GetComponent<TMP_Text>().text = "No Saved Game";
            startGame.transform.Find("Button1").GetComponent<Image>().color = Color.gray;
        }
        else
        {
            startGame.transform.Find("SubText").GetComponent<TMP_Text>().text = "You are at: " + SceneManager.GetSceneByBuildIndex(currentLevel).name;
        }

        UpdateTutorialText();
    }

    void Update()
    {
        /**
        switch (tutorialState)
        {
            case TutorialState.Welcome:
                break;
            case TutorialState.LearnGrabBook:
                break;
            case TutorialState.LearnMoving:
                break;
            case TutorialState.LearnFlipPage:
                break;
            case TutorialState.LearnCastSpell:
                break;
            case TutorialState.LearnNewSpell:
                break;
            case TutorialState.Boss:
                break;
            case TutorialState.StartGame:
                break;
            default:
                break;
        }
        **/
    }

    public void UpdateTutorialText()
    {
        string previousButton = "Previous";
        string nextButton = "Next";
        string title = "";
        string text = "";
        switch (tutorialState)
        {
            case TutorialState.Welcome:
                title = "Welcome back!";
                text = "Do you remember how to use me? Grab me first. (Grab the book with your left controller and press trigger with your right controller to continue)";
                previousButton = "Skip";
                break;
            case TutorialState.LearnGrabBook:
                title = "The Book";
                text = "I am everything. If you want to cast spell, you need to carry me (You don't have to continously grab the object. Grab the book to carry it and grab again to release).";
                break;
            case TutorialState.LearnMoving:
                title = "Move Around";
                text = "Look around the room, don't forget to hold me when you walk. (Use the left joystick to move and rotate with your headset or the right joystick)";
                break;
            case TutorialState.LearnFlipPage:
                title = "Flip Page";
                text = "I store all the spells you have learned, flip the pages to switch to the spell you want. (Target the pages with the right controller and grab)";
                break;
            case TutorialState.LearnSpells:
                title = "Spells";
                text = "Spells have different cooldown, casting range, and damage. You have to target the monster for most spells as long as they are in your casting range (you can see the cast range by the laser's length).";
                break;
            case TutorialState.LearnCastSpell:
                title = "Cast Spell";
                text = "Do you see the dummies over there? Choose a spell and test it! (Target the dummy with the right controller and press trigger)";
                break;
            case TutorialState.LearnNewSpell:
                title = "Learn New Spells";
                text = "I also prepared a new spell for you. If learn a spell multiple times, the spell will become stronger! (Grab the page with the right controller and insert it into the book)";
                break;
            case TutorialState.Boss:
                title = "???????";
                text = "What happened? Where is your spells? I guess the dark mage stole your spells, you must take it back!";
                break;
            case TutorialState.StartGame:
                title = "???????";
                text = "Oh No! The dark mage is summoning monsters to attack your castle! You can take your spells back by killing the monsters.";
                nextButton = "Finish";
                break;
            default:
                break;
        }
        tutorial.transform.Find("Title").GetComponent<TMP_Text>().text = title;
        tutorial.transform.Find("SubText").GetComponent<TMP_Text>().text = text;
        tutorial.transform.Find("Next").GetComponentInChildren<TMP_Text>().text = nextButton;
        tutorial.transform.Find("Previous").GetComponentInChildren<TMP_Text>().text = previousButton;

    }
    public void Next()
    {
        if (canNext)
        {
            if (tutorialState == TutorialState.StartGame)
            {
                tutorial.SetActive(false);
            }
            else
            {
                tutorialState++;
                UpdateTutorialText();
            }
        }
    }

    public void Previous()
    {
        if (tutorialState == TutorialState.Welcome)
        {
            tutorial.SetActive(false);
        }
        else
        {
            tutorialState--;
            UpdateTutorialText();
        }
    }
    public void ProgressToSavedLevel()
    {
        if (player.GetCurrentLevelProgression() != 0)
        {
            SceneManager.LoadScene(player.GetCurrentLevelProgression(), LoadSceneMode.Single);
        }
    }

    public void StartNewGame()
    {
        player.Save(1);
        SceneManager.LoadScene(1);
    }
}
