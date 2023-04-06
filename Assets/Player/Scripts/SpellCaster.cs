using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;
using System;


[Serializable]
public class LearnedSpell
{
    public Spell spell;
    public int level;
    public float cooldownTimer;
    public LearnedSpell(Spell s)
    {
        spell = s;
        level = 1;
        cooldownTimer = 0;
    }

    public bool CanCast()
    {
        return cooldownTimer > spell.cooldown;
    }

    public float GetCooldown()
    {
        return spell.cooldown - cooldownTimer;
    }

    public void CastSpell(GameObject caster, GameObject target, RaycastHit hit)
    {
        spell.OnCast(caster, target, hit);
        cooldownTimer = 0;
    }

}
public class SpellCaster : MonoBehaviour
{

    public int exp = 0;
    public int maxExp = 10;
    public List<LearnedSpell> spells;
    public List<Spell> spellPool;
    public bool isUpgrading = false;
    public GameObject rewardPoints;
    public GameObject rewardPage;
    public AudioClip rewardPageDestroySound;
    public GameObject bookObject;
    public GameObject rayController;
    public XRDirectInteractor directController;

    protected SpellBookManager spellBook;
    protected XRGrabInteractable interactable;

    void Start()
    {
        if (bookObject == null)
        {
            bookObject = GameObject.Find("Book");
        }
        spellBook = bookObject.GetComponent<SpellBookManager>();
        interactable = bookObject.GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // update right hand controller
        rayController.SetActive(interactable.isSelected && !isUpgrading);
        rayController.GetComponent<XRRayInteractor>().interactionLayers = GetSelectedSpell().rayCastLayer;
        rayController.GetComponent<XRRayInteractor>().lineType = GetSelectedSpell().lineType;
        directController.enabled = !rayController.activeSelf;
        // update cooldown
        foreach (LearnedSpell ls in spells)
        {
            ls.cooldownTimer += Time.deltaTime;
        }
        // update exp and show rewards on upgrade
        if (exp >= maxExp)
        {
            exp = 0;
            isUpgrading = true;
            GenerateRewardPage();
        }
    }

    public void CastSpell(SelectEnterEventArgs eventArgs)
    {
        GameObject interactable = eventArgs.interactableObject.transform.gameObject;
        if (rayController.activeSelf && !interactable.CompareTag("Ignore Spell"))
        {
            RaycastHit hit;
            rayController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out hit);
            if (spells[spellBook.selectedSpell].GetCooldown() <= 0)
            {
                spells[spellBook.selectedSpell].CastSpell(rayController, interactable, hit);
            }
        }
    }



    public Spell GetSelectedSpell()
    {
        return spells[spellBook.selectedSpell].spell;
    }

    public void GainEXP(int xp)
    {
        exp += xp;
    }
    public void RefreshRewardPages()
    {
        GameObject[] pages = GameObject.FindGameObjectsWithTag("RewardPage");
        foreach (GameObject page in pages)
        {
            GameObject.Destroy(page);
            AudioSource.PlayClipAtPoint(rewardPageDestroySound, page.transform.position);
        }
    }
    private void GenerateRewardPage()
    {
        List<Spell> pool = new List<Spell>();
        pool.AddRange(spellPool);
        for (int i = 0; i < 3; i++)
        {
            GameObject rp = Instantiate(rewardPage, rewardPoints.transform.GetChild(i).transform.position, rewardPoints.transform.GetChild(i).transform.rotation);
            rp.transform.SetParent(bookObject.transform);
            int randSpell = UnityEngine.Random.Range(0, pool.Count);
            pool.RemoveAt(randSpell);
            rp.GetComponent<RewardPage>().spell = pool[randSpell];
            rp.GetComponent<RewardPage>().level = GetLearnedSpell(pool[randSpell]) == null ? 1 : GetLearnedSpell(pool[randSpell]).level + 1;
        }
    }

    public void LearnSpell(Spell spell)
    {
        spellBook.AddPage();
        LearnedSpell ls = GetLearnedSpell(spell);
        if (ls != null)
        {
            ls.level = ls.level + 1;
        }
        else
        {
            spells.Add(new LearnedSpell(spell));
        }
        RefreshRewardPages();
        isUpgrading = false;
    }

    public LearnedSpell GetLearnedSpell(Spell spell)
    {
        return spells.Find(ls => ls.spell == spell);
    }
}
