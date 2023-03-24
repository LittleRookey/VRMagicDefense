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
    protected PlayerAttributes player;
    public AudioClip turnPageSound;
    public AudioClip flipSound;

    public InputActionReference changeSpell;
    public int selectedSpell = 0;

    public GameObject turnPageLeft;
    public GameObject turnPageRight;
    public GameObject textPanel;

    public Material pageLeftPrefab;
    public Material pageRightPrefab;

    public int pagePerTurn = 4;
    public EndlessBook.PageTurnTimeTypeEnum turnTimeType = EndlessBook.PageTurnTimeTypeEnum.TotalTurnTime;
    public float turnTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        player = gameObject.GetComponent<PlayerAttributes>();

        interactable = gameObject.GetComponent<XRGrabInteractable>();
        changeSpell.action.started += ChangeSpell;
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
        turnPageLeft.SetActive(interactable.isSelected);
        turnPageRight.SetActive(interactable.isSelected);
        textPanel.SetActive(interactable.isSelected && !book.IsTurningPages);
        textPanel.transform.GetChild(0).GetComponent<TMP_Text>().text = player.spells[selectedSpell].displayName;
        textPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = player.spells[selectedSpell].description;
        //pagePerTurn = book.LastPageNumber / 2;
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