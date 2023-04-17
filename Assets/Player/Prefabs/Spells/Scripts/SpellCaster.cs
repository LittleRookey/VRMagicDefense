using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using echo17.EndlessBook;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;
using TMPro;
using System;
using System.IO;
using UnityEngine.SceneManagement;


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
        cooldownTimer = s.GetCooldown(level);
    }

    public bool CanCast()
    {
        return cooldownTimer > spell.GetCooldown(level);
    }

    public float GetCooldown()
    {
        return spell.GetCooldown(level) - cooldownTimer;
    }

    public void CastSpell(GameObject caster, RaycastHit hit)
    {
        spell.OnCast(caster, hit, level);
        Debug.Log("spellcast:" + spell.GetCooldown(level));
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
    public InputActionReference castSpellAction;
    public InputActionReference changeSpellAction;
    public bool disableGameSaving = false;

    public string saveFileLocation;
    public bool saved;
    public AudioClip SaveSound;

    protected SpellBookManager spellBook;
    protected XRGrabInteractable interactable;

    void Awake()
    {
        saveFileLocation = Application.persistentDataPath;
        LoadPlayer();
        castSpellAction.action.started += CastSpell;
        changeSpellAction.action.started += SwitchSpell;
    }

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
        rayController.GetComponent<XRRayInteractor>().interactionLayers = GetSelectedSpell().spell.rayCastLayer;
        rayController.GetComponent<XRRayInteractor>().lineType = GetSelectedSpell().spell.lineType;
        rayController.GetComponent<XRRayInteractor>().maxRaycastDistance = GetSelectedSpell().spell.GetDistance(GetSelectedSpell().level);

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
        if (castSpellAction.action.IsInProgress())
        {
            CastSpell(false);
        }
    }

    public void CastSpell(bool warnCooldown)
    {
        if (rayController != null && rayController.activeSelf)
        {
            if (GetSelectedSpell().spell is SelfSpell)
            {
                if (GetSelectedSpell().GetCooldown() <= 0)
                {
                    GetSelectedSpell().CastSpell(rayController, new RaycastHit());
                }
            }
            else
            {
                RaycastHit hit;
                if (rayController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out hit))
                {
                    if (!hit.transform.gameObject.CompareTag("RewardPage") && !hit.transform.gameObject.CompareTag("Ignore Spell") && !hit.transform.gameObject.CompareTag("Player") && GetSelectedSpell().GetCooldown() <= 0)
                    {
                        GetSelectedSpell().CastSpell(rayController, hit);
                    }
                }

            }
        }
    }

    public void CastSpell(InputAction.CallbackContext context)
    {
        CastSpell(true);
    }

    public void SwitchSpell(InputAction.CallbackContext context)
    {
        if (rayController != null && rayController.activeSelf)
        {
            RaycastHit hit;
            if (rayController.GetComponent<XRRayInteractor>().TryGetCurrent3DRaycastHit(out hit))
            {
                if (hit.transform.gameObject.name == "TouchpadRight")
                {
                    if (spellBook.menu)
                    {
                        AudioSource.PlayClipAtPoint(rewardPageDestroySound, transform.position);
                        if (spellBook.confirmLobby == 1)
                        {
                            ReturnToLobby();
                        }
                        spellBook.confirmLobby = 1;
                    }
                    else
                    {
                        spellBook.TurnPageRight();
                    }
                }
                if (hit.transform.gameObject.name == "TouchpadLeft")
                {
                    if (spellBook.menu)
                    {
                        spellBook.ToggleMenuPage(new InputAction.CallbackContext());
                    }
                    else
                    {
                        spellBook.TurnPageLeft();
                    }
                }
            }
        }
    }

    public LearnedSpell GetSelectedSpell()
    {
        return spells[spellBook.selectedSpell];
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
    public void GenerateRewardPage()
    {
        RefreshRewardPages();
        List<Spell> pool = new List<Spell>();
        pool.AddRange(spellPool);
        for (int i = 0; i < 3; i++)
        {
            GameObject rp = Instantiate(rewardPage, rewardPoints.transform.GetChild(i).transform.position, rewardPoints.transform.GetChild(i).transform.rotation);
            rp.transform.SetParent(bookObject.transform);
            int randSpell = UnityEngine.Random.Range(0, pool.Count);
            rp.GetComponent<RewardPage>().spell = pool[randSpell];
            rp.GetComponent<RewardPage>().level = GetLearnedSpell(pool[randSpell]) == null ? 1 : GetLearnedSpell(pool[randSpell]).level + 1;

            pool.RemoveAt(randSpell);
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

    public void ReturnToLobby()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void Save(int nextLevel)
    {
        if (disableGameSaving) return;
        Properties properties = new Properties();
        properties.spellsLearned = spells;
        properties.spellPool = spellPool;
        properties.buffs = gameObject.GetComponent<BuffReceiver>().buffs;
        properties.exp = exp;
        properties.currentLevel = nextLevel;

        string jsonString = JsonUtility.ToJson(properties);
        Debug.Log("saving " + jsonString);
        File.WriteAllText(saveFileLocation + "/player.txt", jsonString);
        saved = true;
        AudioSource.PlayClipAtPoint(SaveSound, transform.position);
    }

    public int GetCurrentLevelProgression()
    {
        if (File.Exists(saveFileLocation + "/player.txt") && !disableGameSaving)
        {
            string jsonString = File.ReadAllText(saveFileLocation + "/player.txt");
            Properties properties = JsonUtility.FromJson<Properties>(jsonString);
            return properties.currentLevel;
        }
        return 0;
    }

    public void LoadPlayer()
    {
        if (File.Exists(saveFileLocation + "/player.txt") && !disableGameSaving)
        {
            string jsonString = File.ReadAllText(saveFileLocation + "/player.txt");
            Properties properties = JsonUtility.FromJson<Properties>(jsonString);
            spells = properties.spellsLearned;
            spellPool = properties.spellPool;
            exp = properties.exp;
            gameObject.GetComponent<BuffReceiver>().buffs = properties.buffs;
        }
    }
}

[Serializable]
public class Properties
{
    public List<LearnedSpell> spellsLearned;
    public List<Spell> spellPool;
    public int currentLevel;

    public List<BuffInstance> buffs;
    public int exp;
}
