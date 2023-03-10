using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "ScriptableObjects/ProjectileSpell")]
public class ProjectileSpell : Spell
{
    public GameObject projectilePrefab;
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;

    public int spellDamage = 0;
    public float projectileSpeed = 1f;

    public bool isHoming = false;
    public bool isAOE = false;
    public float areaSize = 1;

    public override void OnCast(GameObject caster, GameObject target, RaycastHit hit)
    {
        GameObject projectile = Instantiate(projectilePrefab, caster.transform.position, caster.transform.rotation);
        ProjectileSpellEffect spellEffect = projectile.GetComponent<ProjectileSpellEffect>();
        if (spellEffect == null)
        {
            spellEffect = projectile.AddComponent<ProjectileSpellEffect>();
        }

        spellEffect.caster = caster;
        spellEffect.target = target;
        spellEffect.isHoming = isHoming;
        spellEffect.projectileSpeed = projectileSpeed;
        spellEffect.onHit = OnHitTarget;
    }

    public override void OnHitTarget(SpellEffect spellEffect, GameObject caster, GameObject target, Vector3 hitPoint)
    {
        GameObject hitEffect = Instantiate(hitEffectPrefab, target.transform.position, target.transform.rotation);
        if (hitEffect.GetComponent<HitEffectLifetime>() == null)
        {
            hitEffect.AddComponent<HitEffectLifetime>();
        }
        if (isAOE)
        {
            Collider[] colliders = Physics.OverlapSphere(hitPoint, areaSize, 1 << 7);
            foreach (Collider collider in colliders)
            {
                Health health = collider.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(spellDamage);
                }
            }
        }
        else
        {
            Health health = target.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(spellDamage);
            }
        }
        AudioSource.PlayClipAtPoint(hitSound, target.transform.position);
    }
}
