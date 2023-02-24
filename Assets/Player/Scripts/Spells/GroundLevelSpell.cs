using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GroundLevelSpell", menuName = "ScriptableObjects/GroundLevelSpell")]
public class GroundLevelSpell : Spell
{
    public GameObject spellEffectPrefab;
    public GameObject hitEffectPrefab;

    public AudioClip hitSound;
    public int spellDamage = 0;

    public float delayTime = 0;
    public float repeatInterval = -1;
    public int repeatMax = -1;

    public bool isAOE = false;
    public Vector3 areaSize = new Vector3(1, 1, 1);

    public override void OnCast(GameObject caster, GameObject target, RaycastHit hit)
    {
        int layerMask = 1 << 6;
        RaycastHit groundHit;
        Vector3 raycastPos;
        if (isAOE)
        {
            raycastPos = new Vector3(hit.point.x, hit.point.y + 0.1f, hit.point.z);
        }
        else
        {
            raycastPos = new Vector3(target.transform.position.x, target.transform.position.y + 0.1f, target.transform.position.z);
        }
        if (Physics.Raycast(raycastPos, Vector3.down, out groundHit, Mathf.Infinity, layerMask))
        {
            Quaternion rotation = spellEffectPrefab.transform.rotation;
            rotation.eulerAngles = new Vector3(rotation.eulerAngles.x, caster.transform.rotation.eulerAngles.y, rotation.eulerAngles.z);
            //Vector3 pos = new Vector3(target.transform.position.x, , target.transform.position.z);
            GameObject spell = Instantiate(spellEffectPrefab, groundHit.point, rotation);
            DelayedSpellEffect spellEffect = spell.GetComponent<DelayedSpellEffect>();
            if (spellEffect == null)
            {
                spellEffect = spell.AddComponent<DelayedSpellEffect>();
            }
            spellEffect.caster = caster;
            spellEffect.target = target;
            spellEffect.hitPoint = hit.point;
            spellEffect.delayTime = delayTime;
            spellEffect.repeatInterval = repeatInterval;
            spellEffect.repeatMax = repeatMax;
            spellEffect.onHit = OnHitTarget;
        }

    }

    public override void OnHitTarget(SpellEffect spellEffect, GameObject caster, GameObject target, Vector3 hitPoint)
    {
        if (isAOE)
        {
            Collider[] colliders = Physics.OverlapBox(hitPoint, areaSize / 2, spellEffect.transform.rotation, 1 << 7);
            foreach (Collider collider in colliders)
            {
                GameObject hitEffect = Instantiate(hitEffectPrefab, collider.transform.position, collider.transform.rotation);
                if (hitEffect.GetComponent<HitEffectLifetime>() == null)
                {
                    hitEffect.AddComponent<HitEffectLifetime>();
                }

                Health health = collider.gameObject.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(spellDamage);
                }
            }
        }
        else
        {
            GameObject hitEffect = Instantiate(hitEffectPrefab, target.transform.position, target.transform.rotation);
            if (hitEffect.GetComponent<HitEffectLifetime>() == null)
            {
                hitEffect.AddComponent<HitEffectLifetime>();
            }

            Health health = target.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(spellDamage);
            }
        }
    }

}
