using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class PassiveEnemyScript : MonoBehaviour
{
    
    private Vector3 _leftLimitPosition;
    private Vector3 _rightLimitPosition;
    private bool _goingRight = true;
    private NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        var rightLimit = gameObject.transform.Find("RightLimit");
        _rightLimitPosition = rightLimit.position;

        var leftLimit = gameObject.transform.Find("LeftLimit");
        _leftLimitPosition = leftLimit.position;
    }

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.SetDestination(_goingRight ? _rightLimitPosition : _leftLimitPosition);
        if (Math.Abs(transform.position.x - _leftLimitPosition.x) < 0.5)
        {
            _goingRight = true;
        }
        
        
        if (Math.Abs(transform.position.x - _rightLimitPosition.x) < 0.5)
        {
            _goingRight = false;
        }
        
    }
    
}
