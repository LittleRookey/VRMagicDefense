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
        if (!collider.CompareTag("Ignore Spell") && !collider.CompareTag("Player"))
        {
            Debug.Log(collider.ClosestPoint(transform.position));
            onHit(this, caster, collider.gameObject, collider.ClosestPoint(transform.position), level);
            Destroy(gameObject);
        }
    }

    public override void UpdateSpellEffect()
    {
        if (isHoming)
        {
            if (target != null)
            {
                transform.LookAt(new Vector3(target.transform.position.x, target.transform.position.y + 0.5f, target.transform.position.z));
            }
        }
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime * 10);

    }

}
