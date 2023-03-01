using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Projectile : MonoBehaviour
{

    public UnityAction OnHitTarget;

    public Vector3 shootDir { private set; get; }
    public float moveSpeed { private set; get; }
    public string enemyTag { private set; get; }
    public float damage { private set; get; }

    public void Setup(Vector3 shootDir, float projectileMoveSpeed, string enemTag)
    {
        this.shootDir = shootDir;
        this.moveSpeed = projectileMoveSpeed;
        this.enemyTag = enemTag;
        transform.DOMove(transform.position + shootDir * this.moveSpeed, 3f).SetEase(Ease.Linear);
        Destroy(gameObject, 3f);
    }

    public void Setup(Vector3 shootDir, float projectileMoveSpeed, string enemTag, float damage)
    {
        this.shootDir = shootDir;
        this.moveSpeed = projectileMoveSpeed;
        this.enemyTag = enemTag;
        this.damage = damage;
        transform.DOMove(transform.position + shootDir * this.moveSpeed, 3f).SetEase(Ease.Linear);
        Destroy(gameObject, 3f);
    }

    public void Setup(Vector3 shootDir, float projectileMoveSpeed, string enemTag, float damage, GameObject onHitEffect)
    {
        this.shootDir = shootDir;
        this.moveSpeed = projectileMoveSpeed;
        this.enemyTag = enemTag;
        this.damage = damage;
        transform.DOMove(transform.position + shootDir * this.moveSpeed, 3f).SetEase(Ease.Linear);
        OnHitTarget += () =>
        {
            var onhit = Instantiate(onHitEffect, transform.position, Quaternion.identity);
            Destroy(onhit, 2f);
        };
        Destroy(gameObject, 3f);
    }

    public ProjectileSettings GetProjectileSettings()
    {
        return new ProjectileSettings(shootDir, moveSpeed, enemyTag, damage);
    }

    private void Update()
    {
        //transform.position += shootDir * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnHitTarget?.Invoke();
        if (collision.CompareTag(enemyTag))
        {
            //Debug.Log("Hit " + collision.tag);
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(this.damage);
            }
            DisableSelf();
        }
    }

    private void DisableSelf()
    {
        OnHitTarget = null;
        Destroy(gameObject);
    }
}

[System.Serializable]
public class ProjectileSettings
{
    public Vector3 shootDir { private set; get; }
    public float moveSpeed { private set; get; }
    public string enemyTag { private set; get; }
    public float damage { private set; get; }

    public ProjectileSettings(Vector3 shootDir, float moveSpeed, string enemyTag, float damage)
    {
        this.shootDir = shootDir;
        this.moveSpeed = moveSpeed;
        this.enemyTag = enemyTag;
        this.damage = damage;
    }
}