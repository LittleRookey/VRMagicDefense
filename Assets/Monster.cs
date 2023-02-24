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
    public float _attackRange; // attacks the target when target is within attack range
    Rigidbody rb;

    [SerializeField] private eBehaveState currentState;

    public UnityAction<GameObject> OnIdle;
    public UnityAction<GameObject> OnChase;
    public UnityAction<GameObject> OnAttack;
    public UnityAction<GameObject> OnDeath;

    public bool canMove;

    private float distToTarget;
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
    }

    public void SetTarget(Transform targ)
    {
        _target = targ;
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
                transform.position = Vector3.MoveTowards(transform.position, _target.position, 3f * Time.deltaTime);
                if (distToTarget < _attackRange)
                {
                    // if enemy is within range
                    ChangeState(eBehaveState.Attack);
                }
                OnChase?.Invoke(gameObject);
                break;
            case eBehaveState.Attack:
                // attack the enemy
                OnAttack?.Invoke(gameObject);
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
