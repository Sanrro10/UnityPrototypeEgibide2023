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
        //Guardar la velocidad de la piedra
        rbVelocity = Rb.velocity;
        
        //La piedra ha dejado de moverse
        if (isMoving && rbVelocity == Vector3.zero)
        {
            //Borrar la piedra
            Destroy(gameObject);            
        }
        else if (!isMoving && rbVelocity != Vector3.zero)
        {
            isMoving = true;
        }
    }
}
