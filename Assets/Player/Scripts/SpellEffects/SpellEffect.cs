using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class SpellEffect : MonoBehaviour
{
    public GameObject target;
    public GameObject caster;
    public Vector3 hitPoint;
    public float lifeTime = 10f;

    public HitEffect onHit;
    protected float existTime;

    void Update()
    {
        existTime += Time.deltaTime;
        if (existTime >= lifeTime)
        {
            Destroy(gameObject);
        }
        UpdateSpellEffect();
    }

    public abstract void UpdateSpellEffect();

    public delegate void HitEffect(SpellEffect spellEffect, GameObject caster, GameObject target, Vector3 hitPoint);
}
