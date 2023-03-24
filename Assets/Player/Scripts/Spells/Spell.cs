using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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
}
