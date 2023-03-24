using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DamageNumbersPro;
using Litkey.Utility;


public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private DamageNumberMesh dmgMesh;
    public bool hasHealthBar;
    public bool destroyOnDeath;


    public HealthUI healthUI;

    private float _currentHealth;

    public UnityAction OnSpawn;
    public UnityAction<float> OnHit;
    public UnityAction<GameObject> OnDeath;
    Transform player;
    string dmgPath = "DamageNumber";
    Vector3 oneEighty = new Vector3(0f, 180f, 0f);
    public Vector3 dmgPosOffset;

    private void Awake()
    {
        oneEighty = new Vector3(0f, 180f, 0f);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        if (dmgMesh == null)
        {
            dmgMesh = Resources.Load<DamageNumberMesh>(dmgPath);
        }
        SpawnHealthBar();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
        OnDeath += GainEXP;
    }

    public virtual bool TakeDamage(float dmg)
    {

        _currentHealth -= dmg;
        if (dmgMesh)
        {
            DamageNumber dm;
            if (CompareTag("Castle"))
            {
                dm = dmgMesh.Spawn(transform.position + Vector3.up + dmgPosOffset, dmg, Color.red);

            }
            else
            {
                dm = dmgMesh.Spawn(transform.position + Vector3.up + dmgPosOffset, dmg);
            }

            dm.transform.LookAt(player);
            Vector3 rot = dm.transform.rotation.eulerAngles;
            dm.transform.rotation = Quaternion.Euler(rot + oneEighty);

        }

        OnHit?.Invoke(_currentHealth / _maxHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0f;
            OnHit?.Invoke(_currentHealth / _maxHealth);

            OnDeath?.Invoke(this.gameObject);

            if (destroyOnDeath)
            {
                Destroy(gameObject, 0.1f);
                return false;
            }
            return false;
        }
        return true;
    }

    public void SetHealth(float healthAmount)
    {
        if (healthAmount > _maxHealth)
            healthAmount = _maxHealth;

        this._currentHealth = healthAmount;
        OnHit?.Invoke(this._currentHealth / this._maxHealth);
    }

    public void SetMaxHealth()
    {
        this._currentHealth = this._maxHealth;
        OnHit?.Invoke(this._currentHealth / this._maxHealth);
    }

    public void SetScript(float healthAmount, bool destroyOnZero, bool hasHealthBar)
    {
        this._maxHealth = healthAmount;
        this._currentHealth = _maxHealth;
        this.destroyOnDeath = destroyOnZero;
        this.hasHealthBar = hasHealthBar;

        OnHit?.Invoke(_currentHealth / _maxHealth);
        SpawnHealthBar();
    }

    private void SpawnHealthBar()
    {
        if (hasHealthBar)
        {
            healthUI = transform.GetComponentInChildren<HealthUI>();
            if (healthUI == null)
            {
                healthUI = Instantiate(Resources.Load<HealthUI>("UI/HealthBarCanvas"), transform);
            }
        }
    }

    public bool IsAlive()
    {
        return _currentHealth > 0f;
    }
    public void GainEXP(GameObject enemy)
    {
        PlayerAttributes player = GameObject.FindObjectOfType<PlayerAttributes>();
        if (player != null)
        {
            player.GainEXP(1);
        }
    }
}

