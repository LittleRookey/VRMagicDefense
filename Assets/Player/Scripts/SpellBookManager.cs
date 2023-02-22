using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.XR.Interaction.Toolkit;

public class SpellBookManager : MonoBehaviour
{

    protected EndlessBook book;
    protected XRGrabInteractable interactable;
    protected float timer = 0;
    public AudioClip turnPageSound;
    public AudioClip flipSound;

    public Spell selectedSpell;
    public GameObject turnPageLeft;
    public GameObject turnPageRight;
    public int pagePerTurn = 4;
    public EndlessBook.PageTurnTimeTypeEnum turnTimeType = EndlessBook.PageTurnTimeTypeEnum.TotalTurnTime;
    public float turnTime = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        book = gameObject.GetComponent<EndlessBook>();
        interactable = gameObject.GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        turnPageLeft.SetActive(interactable.isSelected);
        turnPageRight.SetActive(interactable.isSelected);

        if (!interactable.isSelected)
        {
            if (book.CurrentLeftPageNumber == book.LastPageNumber - 1)
            {
                book.SetPageNumber(1);
            }
            timer += Time.deltaTime;

            if (timer >= turnTime)
            {
                //Debug.Log(book.LastPageNumber + "   " + book.CurrentLeftPageNumber);
                timer -= turnTime;

                book.TurnToPage(Mathf.Max(book.CurrentLeftPageNumber + pagePerTurn, book.LastPageNumber - 1), turnTimeType, turnTime, 1f, null, PlayAudioOnFlip, null);

                //book.TurnForward(turnTime);
            }
        }
    }


    public void TurnPageLeft()
    {
        book.TurnBackward(turnTime, null, PlayAudioOnTurn, null);
    }

    public void TurnPageRight()
    {
        book.TurnForward(turnTime, null, PlayAudioOnTurn, null);
    }

    public void StopSoundOnSelected()
    {
        if (gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().Stop();
        }
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