using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DelayedSpellEffect : SpellEffect
{
    public float delayTime;
    public float repeatInterval;
    public int repeatMax;
    protected bool triggered = false;
    protected float repeatTimer = 0;
    protected int repeatTime = 0;

    public override void UpdateSpellEffect()
    {
        if (!triggered && existTime >= delayTime)
        {
            onHit(this, caster, target.gameObject, hitPoint, level);
            triggered = true;
        }
        if (triggered && repeatInterval > 0 && repeatTime < repeatMax)
        {
            repeatTimer += Time.deltaTime;
            if (repeatTimer >= repeatInterval)
            {
                onHit(this, caster, target.gameObject, hitPoint, level);
                repeatTimer = 0;
                repeatTime += 1;
            }
        }
    }
}
