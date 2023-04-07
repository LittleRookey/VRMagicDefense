using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

public class SpellBookManager : MonoBehaviour
{
    protected EndlessBook book;
    protected XRGrabInteractable interactable;
    protected float timer = 0;
    protected SpellCaster player;
    public AudioClip turnPageSound;
    public AudioClip flipSound;

    public InputActionReference toggleMenu;
    public int selectedSpell = 0;

    public GameObject turnPageLeft;
    public GameObject turnPageRight;
    public GameObject textPanel;

    public Material pageLeftPrefab;
    public Material pageRightPrefab;
    public bool menu;
    public int pagePerTurn = 4;
    public EndlessBook.PageTurnTimeTypeEnum turnTimeType = EndlessBook.PageTurnTimeTypeEnum.TotalTurnTime;
    public float turnTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        menu = false;
        player = GameObject.FindObjectOfType<SpellCaster>();
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        toggleMenu.action.started += ToggleMenuPage;

        //changeSpell.action.started += ChangeSpell;
        book = gameObject.GetComponent<EndlessBook>();
        for (int i = 0; i < player.spells.Count; i++)
        {
            AddPage();
        }
        book.SetPageNumber(1);

    }
    public void AddPage()
    {
        book.AddPageData(pageLeftPrefab);
        book.AddPageData(pageRightPrefab);
    }
    // Update is called once per frame
    void Update()
    {
        // update text panel active
        turnPageLeft.SetActive(interactable.isSelected);
        turnPageRight.SetActive(interactable.isSelected);
        textPanel.SetActive(interactable.isSelected && !book.IsTurningPages);
        // update spell display
        if (menu)
        {
            textPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = "Learned Spell";
            string spells = "";
            player.spells.ForEach(spell => spells += spell.spell.displayName + " (lvl:" + spell.level + ")\n");
            textPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = spells;
            textPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = "";
            textPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = "";
            textPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = player.saved ? "Saved" : "Save Game";
        }
        else
        {
            textPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = player.GetSelectedSpell().spell.displayName;
            textPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = player.GetSelectedSpell().spell.description;
            float timeRemaining = player.spells[selectedSpell].GetCooldown();
            if (timeRemaining <= 0)
            {
                textPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = string.Format("Ready ({0:0.0}s)", player.GetSelectedSpell().spell.GetCooldown(player.GetSelectedSpell().level));
            }
            else
            {
                textPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = string.Format("CD: {0:0.00}s", timeRemaining);
            }
            textPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = string.Format("EXP: {0} / {1}", player.exp, player.maxExp);
            textPanel.transform.GetChild(4).GetComponent<TMP_Text>().text = string.Format("Level: {0}", player.GetSelectedSpell().level);
        }

        // flip book if not grabbed
        if (!interactable.isSelected)
        {
            if (book.CurrentLeftPageNumber == book.LastPageNumber - 1)
            {
                book.SetPageNumber(1);
            }
            timer += Time.deltaTime;

            if (timer >= turnTime)
            {
                timer -= turnTime;
                if (book.LastPageNumber > 1)
                {
                    book.TurnToPage(Mathf.Min(book.CurrentLeftPageNumber + pagePerTurn, book.LastPageNumber - 1), turnTimeType, turnTime, 1f, null, PlayAudioOnFlip, null);
                }
            }
        }
        else
        {
            book.SetPageNumber(selectedSpell * 2 + 1);
        }
    }


    public void ChangeSpell(InputAction.CallbackContext action)
    {
        selectedSpell++;
        if (selectedSpell >= player.spells.Count)
        {
            selectedSpell = 0;
        }
    }


    public void TurnPageLeft()
    {
        selectedSpell--;
        if (selectedSpell < 0)
        {
            selectedSpell = player.spells.Count - 1;
        }
        book.TurnToPage(selectedSpell * 2 + 1, turnTimeType, turnTime, 1f, null, PlayAudioOnTurn, null);

    }

    public void TurnPageRight()
    {
        selectedSpell++;
        if (selectedSpell >= player.spells.Count)
        {
            selectedSpell = 0;
        }
        book.TurnToPage(selectedSpell * 2 + 1, turnTimeType, turnTime, 1f, null, PlayAudioOnTurn, null);
    }

    public void ToggleMenuPage(InputAction.CallbackContext action)
    {
        if (!interactable.isSelected) return;
        menu = !menu;
        if (menu)
        {
            player.saved = false;
            book.TurnToPage(player.spells.Count * 2 + 1, turnTimeType, turnTime, 1f, null, PlayAudioOnTurn, null);
        }
        else
        {
            book.TurnToPage(selectedSpell * 2 + 1, turnTimeType, turnTime, 1f, null, PlayAudioOnTurn, null);
        }
    }

    public void StopSoundOnSelected()
    {
        gameObject.GetComponent<AudioSource>().Stop();

    }
    public void UpdateSpellOnSelected()
    {
        textPanel.SetActive(true);
    }

    protected void PlayAudioOnTurn(Page page, int pageNumberFront, int pageNumberBack, int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(turnPageSound);
    }

    protected void PlayAudioOnFlip(Page page, int pageNumberFront, int pageNumberBack, int pageNumberFirstVisible, int pageNumberLastVisible, Page.TurnDirectionEnum turnDirection)
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().PlayOneShot(flipSound);
        }
    }

}