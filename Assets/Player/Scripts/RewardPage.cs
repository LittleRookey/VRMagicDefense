using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;
public class RewardPage : MonoBehaviour
{
    public Spell spell;
    public int level;
    public SpellBookManager book;
    public Vector3 pos;
    public Quaternion rotation;

    protected SpellCaster player;
    protected XRGrabInteractable interactable;

    // Start is called before the first frame update
    void Start()
    {
        interactable = GetComponent<XRGrabInteractable>();
        pos = transform.position;
        rotation = transform.rotation;
        player = GameObject.FindObjectOfType<SpellCaster>();
        book = GameObject.Find("Book").GetComponent<SpellBookManager>();
        transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = spell.displayName;
        transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = spell.description;
        transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = string.Format("Cooldown: {0}s", spell.cooldown);
        transform.GetChild(0).GetChild(3).GetComponent<TMP_Text>().text = string.Format("Level: {0}", level);
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(gameObject.transform.position, book.transform.position) < 0.1)
        {
            player.LearnSpell(spell);
        }
    }
}
