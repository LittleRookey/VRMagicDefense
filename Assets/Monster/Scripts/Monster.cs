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
    enum eBehaveState
    {
        Idle,
        Chase,
        Attack,
        Die
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        currentState = eBehaveState.Chase;
        canMove = true;
        currentTimer = attackTimer;
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

                OnIdle?.Invoke(gameObject);
                break;
            case eBehaveState.Chase:
                if (!canMove) return;

                distToTarget = Vector3.Distance(_target.position, transform.position);
                if (!moveStraight) 
                    transform.position = Vector3.MoveTowards(transform.position, _target.position, moveSpeed * Time.deltaTime);
                else
                    transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.forward * 3f, moveSpeed * Time.deltaTime);
                if (distToTarget <= _attackRange)
                {
                    // if enemy is within range
                    ChangeState(eBehaveState.Attack);
                }
                OnChase?.Invoke(gameObject);
                break;
            case eBehaveState.Attack:
                // attack the enemy
                distToTarget = Vector3.Distance(_target.position, transform.position);
                currentTimer -= Time.deltaTime;

                if (distToTarget <= _attackRange && currentTimer <= 0)
                {
                    currentTimer = attackTimer;
                    _target.GetComponent<Health>().TakeDamage(attackDmg);
                    OnAttack?.Invoke(gameObject);
                }
                else if (distToTarget > _attackRange) // if enemy is out of range, chase
                {
                    ChangeState(eBehaveState.Chase);
                }
                break;
            case eBehaveState.Die:
                OnDeath?.Invoke(gameObject);
                break;
        }
    }

    void ChangeState(eBehaveState state)
    {
        currentState = state;
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
