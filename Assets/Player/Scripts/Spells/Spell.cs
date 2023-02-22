using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[CreateAssetMenu(fileName = "Spell", menuName = "ScriptableObjects/Spell")]
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
    // Start is called before the first frame update
    public virtual void OnCast(GameObject caster, GameObject hit)
    {
        Debug.Log("Hit " + hit.name);
    }
}
