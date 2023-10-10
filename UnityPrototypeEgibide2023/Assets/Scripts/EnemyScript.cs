using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private GameObject player;
    // [SerializeField] private float speed = 0.05f;
    // [SerializeField] private float maxSpeed = 1f;
    // private Rigidbody2D _rigidbody2D;
    // private Vector2 position;
    private NavMeshAgent _navMeshAgent;
    
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        // _rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        _navMeshAgent.SetDestination(player.transform.position);
        //transform.LookAt(player.transform);
        //position = new Vector2(player.transform.position.x, player.transform.position.y);
        // _rigidbody2D.AddForce((player.transform.position - transform.position) * speed);
        //transform.position = Vector2.MoveTowards(transform.position, position, speed * Time.deltaTime);
        // _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, maxSpeed);
    }
}
