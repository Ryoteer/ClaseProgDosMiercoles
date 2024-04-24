using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [Header("<color=red>AI</color>")]
    [SerializeField] private Transform _target;
    [SerializeField] private float _stoppingDist = 2f;

    [Header("<color=red>Values</color>")]
    [SerializeField] private int _maxHP = 100;

    private int _actualHP;

    private NavMeshAgent _agent;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _actualHP = _maxHP;
    }

    private void Update()
    {
        if((_target.position - transform.position).sqrMagnitude 
            <= Mathf.Pow(_stoppingDist, 2))
        {
            if(!_agent.isStopped) _agent.isStopped = true;

            transform.LookAt(_target);
        }
        else if (_agent.isStopped)
        {
            _agent.isStopped = false;
        }
    }

    private void FixedUpdate()
    {
        _agent.SetDestination(_target.position);
    }

    public void SetHP(int modifier)
    {
        _actualHP += modifier;

        if(modifier < 0)
        {
            print($"<color=orange>{name}</color> recibió {-modifier} <color=red>puntos de daño</color>.");
        }
        else
        {
            print($"<color=orange>{name}</color> se <color=green>curó</color> {modifier} puntos de vida.");
        }

        if(_actualHP > _maxHP)
        {
            _actualHP = _maxHP;
        }
        else if(_actualHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
