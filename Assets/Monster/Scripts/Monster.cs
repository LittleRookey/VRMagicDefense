using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Pathfinding;

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

    private AIDestinationSetter aiDest;
    private AIPath aiPath;

    public bool canMove;

    private float distToTarget;
    private float currentTimer;

    protected float idleTimer = 1f;
    protected float idle_countTimer = 0f;
    Animator anim;

    bool isAttacking;

    [SerializeField] AudioClip attackSFX;
    AudioSource audioSource;

    private GameObject castle;

    enum eBehaveState
    {
        Idle,
        Chase,
        Attack,
        Die
    }
    private void Awake()
    {
        aiPath = GetComponent<AIPath>();
        aiDest = GetComponent<AIDestinationSetter>();

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        castle = GameObject.FindGameObjectWithTag("Castle");

        ChangeState(eBehaveState.Chase);
        canMove = true;
        currentTimer = attackTimer;
        audioSource = gameObject.GetComponent<AudioSource>();
        aiPath.endReachedDistance = _attackRange;

        aiPath.maxSpeed = moveSpeed / 6f;
        
        SetTarget(aiDest.target);
    }

    public void SetTarget(Transform targ)
    {
        _target = targ;
        aiDest.target = _target;
        aiPath.canMove = true;
        aiPath.SearchPath();
        ChangeState(eBehaveState.Chase);
    }

    public Transform GetTarget()
    {
        return _target;
    }

    void FindTarget()
    {
        // searches within chase range
        var enemies = Physics.OverlapSphere(transform.position, _chaseRange, LayerMask.GetMask("Ignore Spell"));
        foreach(var enemy in enemies)
        {
            if (enemy.CompareTag("Player"))
            {
                SetTarget(enemy.transform);
            }
        }
        
        //else 
        //    _target = 
    }
    void Act()
    {
        switch (currentState)
        {
            case eBehaveState.Idle:
                if (aiPath.reachedDestination)
                {
                    ChangeState(eBehaveState.Chase);
                    return;
                }
                idle_countTimer += Time.deltaTime;
                if (idle_countTimer >= idleTimer)
                {
                    // Do Something
                    OnIdle?.Invoke(gameObject);
                    idle_countTimer = 0f;
                }
                break;
            case eBehaveState.Chase:
                FindTarget();
                if (aiPath.reachedEndOfPath)
                {
                    Debug.Log("Reached end");
                    aiPath.canMove = false;
                    ChangeState(eBehaveState.Attack);
                }
                break;
            case eBehaveState.Attack:
                if (!_target.gameObject.activeInHierarchy || _target == null)
                {
                    SetTarget(castle.transform);
                    return;
                }
                currentTimer -= Time.deltaTime;
                if (currentTimer <= 0)
                {
                    //transform.LookAt(_target);
                    anim.SetTrigger("Attack");

                }
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
