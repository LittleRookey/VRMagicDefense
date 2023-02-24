using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileSpellEffect : SpellEffect
{
    public bool isHoming;
    public float projectileSpeed;

    void OnTriggerEnter(Collider collider)
    {
        if (!collider.CompareTag("Ignore Spell"))
        {
            onHit(this, caster, collider.gameObject, hitPoint);
            Destroy(gameObject);
        }
    }

    public override void UpdateSpellEffect()
    {
        if (isHoming)
        {
            if (target != null)
            {

            }
        }
        else
        {
            Debug.Log("spell" + projectileSpeed);
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime * 10);
        }
    }

}
