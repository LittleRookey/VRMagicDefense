using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "ScriptableObjects/ProjectileSpell")]
public class ProjectileSpell : Spell
{

    public GameObject projectilePrefab;
    public override void OnCast(GameObject caster, GameObject hit)
    {

    }
}
