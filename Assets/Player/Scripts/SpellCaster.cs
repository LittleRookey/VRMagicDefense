using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;

public class SpellCaster : MonoBehaviour
{
    public GameObject bookObject;
    public GameObject rayController;
    public XRDirectInteractor directController;
    protected float cooldownTimer = 0;
    protected SpellBookManager spellBook;
    protected PlayerAttributes player;
    protected XRGrabInteractable interactable;
    // Start is called before the first frame update
    void Start()
    {
        if (bookObject == null)
        {
            bookObject = GameObject.Find("Book");
        }
        spellBook = bookObject.GetComponent<SpellBookManager>();
        interactable = bookObject.GetComponent<XRGrabInteractable>();
        player = gameObject.GetComponent<PlayerAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        rayController.SetActive(interactable.isSelected);
        rayController.GetComponent<XRRayInteractor>().interactionLayers = player.spells[spellBook.selectedSpell].rayCastLayer;
        rayController.GetComponent<XRRayInteractor>().lineType = player.spells[spellBook.selectedSpell].lineType;
        rayController.GetComponent<XRRayInteractor>().lineType = player.spells[spellBook.selectedSpell].lineType;


        directController.enabled = !interactable.isSelected;
        cooldownTimer += Time.deltaTime;
        float timeRemaining = player.spells[spellBook.selectedSpell].cooldown - cooldownTimer;
        if (timeRemaining <= 0)
        {
            spellBook.textPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = string.Format("Ready ({0:0}s)", player.spells[spellBook.selectedSpell].cooldown);
        }
        else
        {
            spellBook.textPanel.transform.GetChild(2).GetComponent<TMP_Text>().text = string.Format("CD: {0:0.00}s", timeRemaining);
        }
        spellBook.textPanel.transform.GetChild(3).GetComponent<TMP_Text>().text = string.Format("EXP: {0} / {1}", player.exp, player.maxExp);
    }

    public void CastSpell(SelectEnterEventArgs eventArgs)
    {
        GameObject interactable = eventArgs.interactableObject.transform.gameObject;
        if (rayController.activeSelf && !interactable.CompareTag("Ignore Spell"))
        {
            RaycastHit hit;
            rayController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out hit);
            if (cooldownTimer >= player.spells[spellBook.selectedSpell].cooldown)
            {
                player.spells[spellBook.selectedSpell].OnCast(rayController, interactable, hit);
                cooldownTimer = 0;
            }
        }
    }
}
