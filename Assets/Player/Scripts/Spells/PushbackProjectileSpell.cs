using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PushbackProjectileSpell", menuName = "ScriptableObjects/PushbackProjectileSpell")]
public class PushbackProjectileSpell : Spell
{
    public GameObject projectilePrefab;
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;
    public AudioClip castSound;

    public int spellDamage = 0;
    public float projectileSpeed = 1f;

    public bool isHoming = false;
    public float areaSize = 1;

    public override void OnCast(GameObject caster, RaycastHit hit, int level)
    {
        GameObject projectile = Instantiate(projectilePrefab, caster.transform.position, caster.transform.rotation);
        ProjectileSpellEffect spellEffect = projectile.GetComponent<ProjectileSpellEffect>();
        if (spellEffect == null)
        {
            spellEffect = projectile.AddComponent<ProjectileSpellEffect>();
        }

        spellEffect.caster = caster;
        spellEffect.target = hit.transform.gameObject;
        spellEffect.isHoming = isHoming;
        spellEffect.projectileSpeed = projectileSpeed;
        spellEffect.level = level;
        spellEffect.onHit = OnHitTarget;

        if (castSound != null)
        {
            AudioSource.PlayClipAtPoint(castSound, caster.transform.position);
        }
    }

    public override void OnHitTarget(SpellEffect spellEffect, GameObject caster, GameObject target, Vector3 hitPoint, int level)
    {
        GameObject hitEffect = Instantiate(hitEffectPrefab, target.transform.position, target.transform.rotation);
        if (hitEffect.GetComponent<HitEffectLifetime>() == null)
        {
            hitEffect.AddComponent<HitEffectLifetime>();
        }
        Collider[] colliders = Physics.OverlapSphere(hitPoint, areaSize * (0.6f + level * 0.4f), 1 << 7);
        foreach (Collider collider in colliders)
        {
            Health health = collider.gameObject.GetComponent<Health>();
            Monster monster = collider.gameObject.GetComponent<Monster>();
            if (health != null && monster != null)
            {
                Vector3 enemyPos = new Vector3(collider.transform.position.x, 1 + collider.transform.position.y, collider.transform.position.z);
                monster.Push((enemyPos - hitPoint).normalized * 70);
                health.TakeDamage(CalculateDamage(spellDamage, level));
            }
        }

        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, target.transform.position);
        }
    }

    private float CalculateDamage(float baseDamage, int level)
    {
        return Random.Range(spellDamage * 0.9f, spellDamage * 1.1f);
    }

    public override float GetCooldown(int level)
    {
        return cooldown;
    }
}
