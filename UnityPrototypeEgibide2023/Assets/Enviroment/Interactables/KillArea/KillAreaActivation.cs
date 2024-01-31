using System.Collections;
using System.Collections.Generic;
using Entities.Player.Scripts;
using UnityEngine;

public class KillAreaActivation : MonoBehaviour
{
    [SerializeField] private GameObject _killArea1;
    [SerializeField] private GameObject _killArea2;
    
    private void OnTriggerStay2D(Collider2D other)
    {
       if (other.gameObject.CompareTag("Player"))
       {
           if (other.gameObject.GetComponent<PlayerController>().IsGrounded())
           {
               if (_killArea1 != null)
               {
                   _killArea1.SetActive(false); 
               } 
               
               if (_killArea2 != null)
               {
                   _killArea2.SetActive(true); 
               }
               
               
              
           }
        }
        
    }
}
