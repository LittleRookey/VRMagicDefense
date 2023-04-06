using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public enum Buff
{
    SPEED_INCREASE,
    GHOST,
    REGENERATION,
}

[Serializable]
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
public class BuffReceiver : MonoBehaviour
{

    public List<BuffInstance> buffs = new List<BuffInstance>();
    void Update()
    {
        UpdateBuff();
    }

    public void AddBuff(Buff buff, float time, GameObject effectPrefab)
    {
        BuffInstance bi = GetBuff(buff);
        if (bi == null)
        {
            GameObject effect = Instantiate(effectPrefab, transform.position, effectPrefab.transform.rotation);
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

    public void UpdateBuff()
    {
        if (gameObject.GetComponent<ActionBasedContinuousMoveProvider>() != null)
        {
            gameObject.GetComponent<ActionBasedContinuousMoveProvider>().moveSpeed = GetBuff(Buff.SPEED_INCREASE) == null ? 3 : 5;
        }

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

}
