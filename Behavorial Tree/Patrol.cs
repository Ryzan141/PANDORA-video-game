using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class Patrol : MonoBehaviour, IBehaviour
{
    public Action OnStateChanged {get; set;}
    [SerializeField] Transform[] _patrolPoints;
    [SerializeField] float _speed = 1f;
    NavMeshAgent navMeshAgent;

    Transform _target;
    int _targetIndex = 0;

    public bool IsActive => true;
    
    void Start() 
    {
        _target = _patrolPoints[_targetIndex];
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = _speed;
    }
    public void Tick()
    {
        if (navMeshAgent.speed !=_speed)
            navMeshAgent.speed = _speed;
        if(Vector3.Distance(transform.position, _target.position) < 0.5f)
        {
            _targetIndex++;
            if (_targetIndex >= _patrolPoints.Length)
            {
                _targetIndex = 0;
            }
            _target = _patrolPoints[_targetIndex];
        }
        navMeshAgent.destination = _target.position;
        
    }
}
