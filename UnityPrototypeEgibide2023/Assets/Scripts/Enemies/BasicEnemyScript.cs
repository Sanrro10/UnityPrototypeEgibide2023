using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private NavMeshAgent _navMeshAgent;

    private Vector3 origin;
    private bool followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            _navMeshAgent.SetDestination(player.transform.position);
        }
        else
        {
            _navMeshAgent.SetDestination(origin);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            followPlayer = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            followPlayer = false;
        }
    }
}
