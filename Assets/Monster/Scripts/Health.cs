using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    public bool hasHealthBar;
    public bool destroyOnDeath;


    public HealthUI healthUI;

    private float _currentHealth;

    public UnityAction OnSpawn;
    public UnityAction<float> OnHit;
    public UnityAction<GameObject> OnDeath;


    private void Awake()
    {

        SpawnHealthBar();
    }
    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    public virtual void TakeDamage(float dmg)
    {

        _currentHealth -= dmg;

        OnHit?.Invoke(_currentHealth / _maxHealth);

        if (_currentHealth <= 0)
        {
            _currentHealth = 0f;
            OnHit?.Invoke(_currentHealth / _maxHealth);

            OnDeath?.Invoke(this.gameObject);

            if (destroyOnDeath) Destroy(gameObject);
        }
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

}

