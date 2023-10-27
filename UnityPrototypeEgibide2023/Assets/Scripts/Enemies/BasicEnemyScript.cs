using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : EntityControler
{
    [SerializeField] private GameObject player;
    [SerializeField] private BasicEnemyData basicEnemyData;
    private NavMeshAgent _navMeshAgent;
    
    
    
    private Vector3 origin;
    private bool followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", basicEnemyData.health);
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer)
        {
            Debug.Log(player.transform.position);
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
    
    public override void OnDeath()
    {
        //TO-DO Activar animacion de muerte y logica relacionada
        
        Invoke(nameof(DestroyThis),2f);
        
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
}
