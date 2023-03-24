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
            Debug.Log(collider.ClosestPoint(transform.position));
            onHit(this, caster, collider.gameObject, collider.ClosestPoint(transform.position));
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
            transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime * 10);
        }
    }

}
