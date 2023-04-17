using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public enum Buff
{
    SPEED_INCREASE,
    FLY,
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
    public float buffEffectOffset;
    void Update()
    {
        UpdateBuff();
    }

    public void AddBuff(Buff buff, float time, GameObject effectPrefab)
    {
        BuffInstance bi = GetBuff(buff);
        if (bi == null)
        {
            GameObject effect = Instantiate(effectPrefab, new Vector3(transform.position.x, transform.position.y + buffEffectOffset, transform.position.z), effectPrefab.transform.rotation);
            effect.transform.parent = gameObject.transform;
            if (effect.GetComponent<HitEffectLifetime>() == null)
            {
                effect.AddComponent<HitEffectLifetime>();
            }
            effect.GetComponent<HitEffectLifetime>().lifetime = time;
            buffs.Add(new BuffInstance(buff, time, effect));
        }
        else
        {
            bi.timeRemaining = time;
            bi.buffEffect.GetComponent<HitEffectLifetime>().lifetime = time;
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
            gameObject.GetComponent<ActionBasedContinuousMoveProvider>().moveSpeed = GetBuff(Buff.SPEED_INCREASE) == null ? 2 : 5;
            gameObject.GetComponent<ActionBasedContinuousMoveProvider>().enableFly = !(GetBuff(Buff.FLY) == null);
            gameObject.GetComponent<ActionBasedContinuousMoveProvider>().useGravity = GetBuff(Buff.FLY) == null;
            gameObject.GetComponent<Rigidbody>().useGravity = GetBuff(Buff.FLY) == null;
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
