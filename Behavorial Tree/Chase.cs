using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent (typeof(Collider))]
[DisallowMultipleComponent]
public class Chase : MonoBehaviour, IBehaviour
{
    public Action OnStateChanged {get; set;}

    [SerializeField] float _speed = 3f;

    Transform _target = null;
    

    bool _isActive = false;
    bool seen = false;
    Vector3 pointSeen;
    public bool IsActive => _isActive;
    Transform visual;
    Animator animator;

    NavMeshAgent navMeshAgent;
    Attack attack;

    //test stuff
    public float distance;
    public float angle;
    public Transform sees;
    void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        visual = transform.GetChild(0).transform;
        attack = GetComponent<Attack>();
    }
    void OnDrawGizmos()
    {
         // (_isActive && attack.IsActive)
        if (_isActive && seen)
            Gizmos.DrawLine(visual.position, _target.position);
        else if (_isActive)
            Gizmos.DrawLine(visual.position, pointSeen);
    }
    public void Tick()
    {
        
        if (!_isActive || _target == null) return;
        visual.LookAt(_target);
        RaycastHit hit;
        if (Physics.Raycast(visual.position, _target.position-visual.position, out hit, 1000f))
        {
            Debug.Log(hit.transform);
            if (hit.transform != null && hit.transform.CompareTag("Player"))
            {
                navMeshAgent.speed = _speed;
                navMeshAgent.destination = _target.position;

            }
            else
            {
                if (seen)
                {
                    Debug.Log("target lost");
                    seen = false;
                    animator.SetBool("Seen", seen);
                }
                distance = Vector3.Distance(transform.position, navMeshAgent.destination);
                if (Vector3.Distance(transform.position, navMeshAgent.destination) <= 1f && !seen)
                {
                    
                    Debug.Log("searching");
                    transform.forward = Vector3.Slerp(transform.forward, new Vector3(_target.position.x-transform.position.x,0, _target.position.z - transform.position.z), 0.25f);
                    Invoke("Lost", 1f);
                }

            }

        }
    }
    void Update()
    {
        if (_target == null) return;
        angle = Vector3.Angle(transform.forward, _target.position - transform.position);
        if (Vector3.Angle(transform.forward, _target.position - transform.position) < 80 && !seen)
        {
            RaycastHit hit;
            if (Physics.Raycast(visual.position, _target.position - visual.position, out hit, 1000f))
            {
                sees = hit.transform;
                pointSeen = hit.point;

                if (hit.transform != null && hit.transform.CompareTag("Player"))
                {
                    Debug.Log("target seen");
                    _isActive = true;
                    seen = true;
                    animator.SetBool("Seen", seen);
                    OnStateChanged.Invoke();

                }
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Target in Range");
            _target = other.transform;
            
        }
        
    }
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player"))
        {
            if(_target != null)
            {
                if (_target.transform == other.transform) 
                    _target = null;
            }
            Debug.Log("Target out of Range");
            _isActive = false;
            seen = false;
            animator.SetBool("Seen", seen);
            visual.localEulerAngles = Vector3.zero;
            OnStateChanged.Invoke();

            
        }

    }
    void Lost()
    {
        _isActive = false;
        seen = false;
        animator.SetBool("Seen", seen);
        visual.localEulerAngles = Vector3.zero;
        OnStateChanged.Invoke();
    }
}
