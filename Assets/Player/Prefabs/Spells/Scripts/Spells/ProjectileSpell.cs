using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "ScriptableObjects/ProjectileSpell")]
public class ProjectileSpell : Spell
{
    public GameObject projectilePrefab;
    public GameObject hitEffectPrefab;
    public AudioClip hitSound;
    public AudioClip castSound;

    public int spellDamage = 0;
    public float projectileSpeed = 1f;

    public bool isHoming = false;
    public bool isAOE = false;
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
        spellEffect.enemyLayer = spellEffect.target.layer;
        if (castSound != null)
            AudioSource.PlayClipAtPoint(castSound, caster.transform.position);
    }

    public override void OnHitTarget(SpellEffect spellEffect, GameObject caster, GameObject target, Vector3 hitPoint, int level)
    {
        GameObject hitEffect = Instantiate(hitEffectPrefab, target.transform.position, target.transform.rotation);
        if (hitEffect.GetComponent<HitEffectLifetime>() == null)
        {
            hitEffect.AddComponent<HitEffectLifetime>();
        }
        if (isAOE)
        {
            Collider[] colliders = Physics.OverlapSphere(hitPoint, areaSize * (0.8f + level * 0.2f), 1 << 7);
            foreach (Collider collider in colliders)
            {
                Debug.Log("collide");

                Health health = collider.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(CalculateDamage(spellDamage, level));
                }
            }
        }
        else
        {
            Health health = target.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(CalculateDamage(spellDamage, level));
            }
        }
        if (hitSound != null)
            AudioSource.PlayClipAtPoint(hitSound, target.transform.position);
    }

    protected float CalculateDamage(float baseDamage, int level)
    {
        float damage = spellDamage * (0.8f + level * 0.2f);
        return Random.Range(damage * 0.9f, damage * 1.1f);
    }

    public override float GetCooldown(int level)
    {
        return cooldown * (Mathf.Pow(0.9f, level - 1));
    }
}
