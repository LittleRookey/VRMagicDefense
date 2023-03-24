using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{

    [SerializeField] private Transform _target;
    public bool showGizmos;
    public float moveSpeed = 3f;
    public float _chaseRange; // chases for enemy within chase range
    [Header("Attack")]
    [SerializeField] private float attackTimer = 3f;
    [SerializeField] private float attackDmg = 10f;
    public float _attackRange; // attacks the target when target is within attack range
    Rigidbody rb;

    [SerializeField] private eBehaveState currentState;
    [SerializeField] private bool moveStraight;
    public UnityAction<GameObject> OnIdle;
    public UnityAction<GameObject> OnChase;
    public UnityAction<GameObject> OnAttack;
    public UnityAction<GameObject> OnDeath;

    public bool canMove;

    private float distToTarget;
    private float currentTimer;

    protected float idleTimer = 1f;
    protected float idle_countTimer = 0f;
    Animator anim;

    bool isAttacking;

    [SerializeField] AudioClip attackSFX;
    AudioSource audioSource;

    enum eBehaveState
    {
        Idle,
        Chase,
        Attack,
        Die
    }
    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        ChangeState(eBehaveState.Chase);
        canMove = true;
        currentTimer = attackTimer;
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public void SetTarget(Transform targ)
    {
        _target = targ;
    }

    public Transform GetTarget()
    {
        return _target;
    }

    void FindTarget()
    {
        // searches within chase range
        var enemies = Physics.OverlapSphere(transform.position, _chaseRange, LayerMask.GetMask("Unit"));
        if (enemies.Length > 0)
            _target = enemies[0].transform;
        //else 
        //    _target = 
    }
    void Act()
    {
        switch(currentState)
        {
            case eBehaveState.Idle:

                idle_countTimer += Time.deltaTime;
                if (idle_countTimer >= idleTimer)
                {
                    // Do Something
                    OnIdle?.Invoke(gameObject);
                    idle_countTimer = 0f;
                }
                break;
            case eBehaveState.Chase:
                if (!canMove) return;

                distToTarget = Vector3.Distance(_target.position, transform.position);
                Vector3 direction = (_target.position - transform.position).normalized;
                if (!moveStraight)
                {
                    //rb.MovePosition(_target.position);
                    //transform.position = Vector3.MoveTowards(transform.position, _target.position, moveSpeed * Time.deltaTime);
                    // Calculate the direction in which the monster should move

                    // Move the monster towards the target position
                    transform.LookAt(_target.position);
                    rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
                }
                else
                {
                    //transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward * 3f, moveSpeed * Time.deltaTime);
                    //rb.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward * 3f, moveSpeed * Time.deltaTime);
                    direction = Vector3.forward;
                    rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
                }
                    

                if (distToTarget <= _attackRange)
                {
                    // if enemy is within range
                    SetIdle(0f);
                    //ChangeState(eBehaveState.Attack);
                }
                OnChase?.Invoke(gameObject);
                break;
            case eBehaveState.Attack:
                if (isAttacking) return;
                // attack the enemy
                distToTarget = Vector3.Distance(_target.position, transform.position);
                //currentTimer -= Time.deltaTime;

                //if (distToTarget <= _attackRange && currentTimer <= 0)
                //{
                if (distToTarget > _attackRange) // if enemy is out of range, chase
                {
                    ChangeState(eBehaveState.Chase);
                } else
                {
                    transform.LookAt(_target);
                    anim.SetFloat("Run", 0f);
                    audioSource.clip = attackSFX;
                    audioSource.PlayDelayed(1);
                    anim.SetTrigger("Attack");
                    isAttacking = true;
                    //SetIdle(attackTimer);
                }
                    //currentTimer = attackTimer;
                    //_target.GetComponent<Health>().TakeDamage(attackDmg);
                    //OnAttack?.Invoke(gameObject);
                    
                    
                //}
                break;
            case eBehaveState.Die:
                OnDeath?.Invoke(gameObject);
                break;
        }
    }
    void SetIdle(float timer)
    {
        idleTimer = timer;
        idle_countTimer = 0f;
        OnIdle = null;
        OnIdle += ((GameObject go) => ChangeState(eBehaveState.Attack));
        ChangeState(eBehaveState.Idle);
    }

    void AttackEnemy()
    {
        Debug.Log("Attacked Enemy " + _target.name);
        currentTimer = attackTimer;
        // attack the target, and becomes Idle
        Health health = _target.GetComponent<Health>();
        bool isAlive = health.TakeDamage(attackDmg);
        // set idle for few seconds
        if (isAlive)
            SetIdle(attackTimer);
        isAttacking = false;
    }

    void ChangeState(eBehaveState state)
    {

        currentState = state;
        switch (state)
        {
            case eBehaveState.Idle:
                anim.SetFloat("Run", 0f);
                break;
            case eBehaveState.Chase:
                anim.SetFloat("Run", 1f);
                break;
            case eBehaveState.Attack:

                break;
            case eBehaveState.Die:
                anim.SetTrigger("Die");
                break;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _chaseRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
        }
    }
    private void Update()
    {
        if (_target == null) return;

        // Chase Target, attack target if target is within attack range
        Act();
    }
}
