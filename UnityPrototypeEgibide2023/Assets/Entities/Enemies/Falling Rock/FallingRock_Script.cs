using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class FallingRock_Script : EntityControler
{
    private bool isMoving = false;
    private bool asExploded = false;
    private Vector3 rbVelocity;
    [SerializeField] private Rigidbody Rb;
    [SerializeField] GameObject collider;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject explotion;
    private GameObject spawnedExplotion;
 
    void FixedUpdate ()
    {
        //Guardar la velocidad de la piedra
        rbVelocity = Rb.velocity;
        
        //La piedra ha dejado de moverse
        if (isMoving && rbVelocity == Vector3.zero && !asExploded)
        {
            //Instanciate explotion
            asExploded = true;
            spawnedExplotion = Instantiate(explotion, transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            rock.SetActive(false);
            
            //Invoke("DeleteExplotion", 1);
            Invoke("DeleteRock", 1);

                      
        }
        else if (!isMoving && rbVelocity != Vector3.zero)
        {
            isMoving = true;
        }
    }
    
    private void DeleteExplotion()
    { 
        Destroy(spawnedExplotion);
    }    
    
    private void DeleteRock()
    { 
        Destroy(gameObject);
        
    }
    
}
