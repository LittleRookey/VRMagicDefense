using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Buff
{
    SPEED_INCREASE,
    GHOST,
    REGENERATION,
}

public class BuffInstance
{
    public Buff buff;
    public GameObject buffEffect;

    public float timeRemaining;
    public BuffInstance(Buff b, float duration, GameObject effect)
    {
        buff = b;
        timeRemaining = duration;
        buffEffect = effect;
    }
}
public class PlayerAttributes : MonoBehaviour
{
    public int exp = 0;
    public int maxExp = 10;
    public List<BuffInstance> buffs = new List<BuffInstance>();
    public List<Spell> spells;
    public List<Spell> spellPool;

    public GameObject rewardPoints;
    public GameObject rewardPage;
    public AudioClip rewardPageDestroySound;

    void Start()
    {
        buffs.RemoveAll((buff) =>
        {
            buff.timeRemaining -= Time.deltaTime;
            if (buff.timeRemaining <= 0)
            {
                GameObject.Destroy(buff.buffEffect);
                return true;
            }
            return false;
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddBuff(Buff buff, float time, GameObject effectPrefab)
    {
        BuffInstance bi = GetBuff(buff);
        if (bi == null)
        {
            GameObject effect = Instantiate(effectPrefab, new Vector3(0, 0, 0), effectPrefab.transform.rotation);
            effect.transform.parent = gameObject.transform;
            buffs.Add(new BuffInstance(buff, time, effect));
        }
        else
        {
            bi.timeRemaining = time;
        }
    }

    public BuffInstance GetBuff(Buff buff)
    {
        BuffInstance rv = null;
        buffs.ForEach((bi) =>
        {
            if (bi.buff == buff)
            {
                rv = bi;
            }
        });
        return rv;
    }

    public void GainEXP(int xp)
    {
        exp += xp;
        if (exp >= maxExp)
        {
            exp = 0;
            for (int i = 0; i < 3; i++)
            {

                GenerateRewardPage(i);
            }
        }

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
    private GameObject GenerateRewardPage(int count)
    {
        GameObject rp = Instantiate(rewardPage, rewardPoints.transform.GetChild(count).transform.position, rewardPoints.transform.GetChild(count).transform.rotation);
        int spell = Random.Range(0, spellPool.Count);
        rp.GetComponent<RewardPage>().spell = spellPool[spell];
        spellPool.RemoveAt(spell);
        return rp;
    }

}