using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelfSpell", menuName = "ScriptableObjects/SelfSpell")]
public class SelfSpell : Spell
{
    public GameObject spellEffectPrefab;
    public float duration;
    public Buff buff;
    public AudioClip castSound;

    public override void OnCast(GameObject caster, GameObject target, RaycastHit hit)
    {
        PlayerAttributes player = GameObject.FindObjectOfType<PlayerAttributes>();
        if (player != null)
        {
            AudioSource.PlayClipAtPoint(castSound, caster.transform.position);
            player.AddBuff(buff, duration, spellEffectPrefab);
        }
    }
}
