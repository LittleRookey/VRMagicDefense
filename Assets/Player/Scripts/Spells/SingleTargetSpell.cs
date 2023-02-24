using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetSpell", menuName = "ScriptableObjects/SingleTargetSpell")]
public class SingleTargetSpell : Spell
{
    public GameObject hitEffectPrefab;

    public int spellDamage = 0;

    public override void OnCast(GameObject caster, GameObject target, RaycastHit hit)
    {
        GameObject projectile = Instantiate(hitEffectPrefab, target.transform.position, target.transform.rotation);
    }
}
