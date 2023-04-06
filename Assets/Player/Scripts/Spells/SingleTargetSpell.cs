using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleTargetSpell", menuName = "ScriptableObjects/SingleTargetSpell")]
public class SingleTargetSpell : Spell
{
    public GameObject hitEffectPrefab;

    public int spellDamage = 0;

    public override void OnCast(GameObject caster, RaycastHit hit, int level)
    {
        GameObject projectile = Instantiate(hitEffectPrefab, hit.transform.position, hit.transform.rotation);
    }
}
