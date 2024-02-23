using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Entities;
using Unity.XR.Oculus.Input;
using UnityEngine;
using Random = UnityEngine.Random;

public class FallingRock_Script : EntityControler
{
    private bool isMoving = false;
    private bool asExploded = false;
    private Vector3 rbVelocity;
    private CinemachineImpulseSource impulseSource;
    
    
    [SerializeField] GameObject collider;
    [SerializeField] GameObject rock;
    [SerializeField] GameObject explotion;
    [SerializeField] private Rigidbody Rb;
    private GameObject spawnedExplotion;

    private bool destructionCalled = false;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void FixedUpdate ()
    {
        
        Invoke(nameof(DeleteRock), 15);
        
        //Guardar la velocidad de la piedra
        rbVelocity = Rb.velocity;
        
        //La piedra ha dejado de moverse
        if (isMoving && rbVelocity == Vector3.zero && !asExploded)
        {
            //Instanciate explotion
            asExploded = true;
            spawnedExplotion = Instantiate(explotion, transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            rock.SetActive(false);

            if (!destructionCalled)
            {
                //Invoke("DeleteExplotion", 1);
                Invoke("DeleteRock", 1);
                impulseSource.m_ImpulseDefinition.m_ImpulseDuration = Random.Range(0.18f, 0.30f);
                impulseSource.m_DefaultVelocity.y = UnityEngine.Random.Range(0.55f, 1f);
                impulseSource.GenerateImpulseWithForce(0.12f);
                destructionCalled = true;
            }
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
        //Debug.Log("Destruccion!");
    }
    
}
