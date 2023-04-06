using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[Serializable]
public class Spell : ScriptableObject
{
    /// <summary>
    /// The raycast layer of this spell
    /// ~0 = every
    /// 1 = ground
    /// 2 = enemy
    /// </summary>
    public InteractionLayerMask rayCastLayer = ~0;
    public XRRayInteractor.LineType lineType = XRRayInteractor.LineType.StraightLine;
    public string displayName;
    public string description;
    public float cooldown;

    public virtual void OnCast(GameObject caster, GameObject target, RaycastHit hit)
    {
        Debug.Log("Target " + target.name);
    }

    public virtual void OnHitTarget(SpellEffect spellEffect, GameObject caster, GameObject target, Vector3 hitPoint)
    {
        Debug.Log("Hit " + target.name);
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        return displayName == ((Spell)obj).displayName;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return displayName.GetHashCode();
    }
}
