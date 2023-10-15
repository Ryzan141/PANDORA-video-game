using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : MonoBehaviour, IBehaviour
{
    public Action OnStateChanged {get; set;}

    bool isActive = false;
    public bool IsActive => isActive;

    [SerializeField] float _attackRange = 5f, _speed = 3f;
    Transform _target = null;
    Transform visual;
    Chase chase;
    NavMeshAgent navMesh;

    private void Start()
    {
        navMesh = GetComponent<NavMeshAgent>();
        visual = transform.GetChild(0).transform;
        _target = GameObject.FindWithTag("Player").transform;
        chase = GetComponent<Chase>();
    }
    public void Tick()
    {
        navMesh.speed = _speed;
        navMesh.destination = _target.position + (transform.position-_target.position).normalized*2f;
        visual.LookAt (_target);
    }

    void Update() 
    {
        if (Vector3.Distance(transform.position, _target.position) < _attackRange && chase.IsActive == true)
        {
            if (isActive == false)
            {
                isActive = true;
                OnStateChanged.Invoke();
            }

        }
        else
        {
            if(isActive == true)
            {
                isActive = false;
                visual.localEulerAngles = Vector3.zero;
                OnStateChanged.Invoke();
            }
        }
    }
}
