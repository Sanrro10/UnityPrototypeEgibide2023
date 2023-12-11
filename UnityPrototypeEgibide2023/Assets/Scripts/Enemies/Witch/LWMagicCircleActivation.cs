using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LWMagicCircleActivation : MonoBehaviour
 {
     private void OnTriggerEnter2D(Collider2D other)
     {
         //Checks if the player has entered the MagicCircle range
         if (other.CompareTag("Player"))
         {
             gameObject.GetComponentInParent<LandWitch>().SendMessage("SetMagicCirclePossible", true, SendMessageOptions.RequireReceiver);
         }
     }
 
     private void OnTriggerExit2D(Collider2D other)
     {
         //Checks if the player has left the MagicCircle range
         if (other.CompareTag("Player"))
         {
             gameObject.GetComponentInParent<LandWitch>().SendMessage("SetMagicCirclePossible", false, SendMessageOptions.RequireReceiver);
         }
     }
 }
