using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class SpellCaster : MonoBehaviour
{
    public GameObject bookObject;
    public GameObject rayController;
    public XRDirectInteractor directController;

    protected SpellBookManager spellBook;
    protected XRGrabInteractable interactable;
    // Start is called before the first frame update
    void Start()
    {
        if (bookObject == null)
        {
            bookObject = GameObject.FindGameObjectWithTag("Book");
        }
        spellBook = bookObject.GetComponent<SpellBookManager>();
        interactable = bookObject.GetComponent<XRGrabInteractable>();
    }

    // Update is called once per frame
    void Update()
    {
        rayController.SetActive(interactable.isSelected);
        rayController.GetComponent<XRRayInteractor>().interactionLayers = spellBook.selectedSpell.rayCastLayer;
        rayController.GetComponent<XRRayInteractor>().lineType = spellBook.selectedSpell.lineType;
        rayController.GetComponent<XRRayInteractor>().lineType = spellBook.selectedSpell.lineType;

        directController.enabled = !interactable.isSelected;
    }

    public void CastSpell(SelectEnterEventArgs eventArgs)
    {
        GameObject interactable = eventArgs.interactableObject.transform.gameObject;
        if (rayController.activeSelf && !interactable.CompareTag("Book"))
        {
            spellBook.selectedSpell.OnCast(gameObject, interactable);
        }
    }
}
