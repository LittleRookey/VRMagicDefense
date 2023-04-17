using LittleRookey.Character.Cooldowns;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSpell", menuName = "ScriptableObjects/BossProjectileSpell")]
public class BossProjectileSpell : ProjectileSpell, IHasCooldown
{
    public int ID => id;

    [SerializeField] private int id;
    public float CooldownDuration => cooldown;

    public override void OnCast(GameObject caster, GameObject target)
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
        spellEffect.level = 1;
        spellEffect.enemyLayer = target.layer;
        spellEffect.onHit = OnHitTarget;
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
}
