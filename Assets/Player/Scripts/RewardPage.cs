using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardPage : MonoBehaviour
{
    public Spell spell;
    public GameObject destroyEffect;
    public SpellBookManager book;

    // Start is called before the first frame update
    void Start()
    {
        book = GameObject.Find("Book").GetComponent<SpellBookManager>();
        transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = spell.displayName;
        transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = spell.description;
        transform.GetChild(0).GetChild(2).GetComponent<TMP_Text>().text = string.Format("Cooldown: {0}s", spell.cooldown);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Vector3.Distance(gameObject.transform.position, book.transform.position));
        if (Vector3.Distance(gameObject.transform.position, book.transform.position) < 0.5)
        {
            book.AddPage();
            book.spells.Add(spell);
            GameObject[] pages = GameObject.FindGameObjectsWithTag("RewardPage");
            foreach (GameObject page in pages)
            {
                GameObject.Destroy(page);
                Debug.Log("destroy");

                //GameObject hitEffect = Instantiate(destroyEffect, page.transform.position, destroyEffect.transform.rotation);
            }
        }
    }
}
