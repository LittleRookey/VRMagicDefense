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

    public override void OnCast(GameObject caster, RaycastHit hit, int level)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && player.GetComponent<BuffReceiver>() != null)
        {
            AudioSource.PlayClipAtPoint(castSound, caster.transform.position);
            player.GetComponent<BuffReceiver>().AddBuff(buff, duration * (0.5f + level * 0.5f), spellEffectPrefab);
        }
    }
}
