using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{

    [SerializeField] private Transform _target;

    public float _chaseRange;
    public float _attackRange;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetTarget(Transform targ)
    {
        _target = targ;
    }

    private void Update()
    {
        if (_target == null) return;

        // Chase Target, attack target if target is within attack range

    }
}
