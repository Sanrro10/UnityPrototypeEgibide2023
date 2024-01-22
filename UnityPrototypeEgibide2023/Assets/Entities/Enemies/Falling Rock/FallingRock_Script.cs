using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class FallingRock_Script : EntityControler
{
    private bool isMoving = false;
    private Vector3 rbVelocity;
    [SerializeField] private Rigidbody Rb;
    [SerializeField] GameObject collider;
 
    void FixedUpdate ()
    {
        rbVelocity = Rb.velocity;
        if (isMoving && rbVelocity == Vector3.zero)
        {
            //rb has stopped moving
            Debug.Log("la piedra cayo");
            isMoving = false;
            Destroy(gameObject);            
        }
        else if (!isMoving && rbVelocity != Vector3.zero)
        {
            isMoving = true;
        }
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
