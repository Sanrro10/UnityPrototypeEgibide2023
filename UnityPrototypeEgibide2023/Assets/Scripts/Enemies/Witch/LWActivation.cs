using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LWActivation : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        /*Checks if the player has entered the witch's action zone*/
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("SetActiveState" , true, SendMessageOptions.RequireReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /*Checks if the player has left the witch's action zone*/
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("SetActiveState" , false , SendMessageOptions.RequireReceiver);
            
        }
    }
}
