using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LWMissileActivation : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Checks if the player has entered the Missile Range
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("SetMissilePossible", true, SendMessageOptions.RequireReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //Checks if the player has left the Missile Range
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("SetMissilePossible", false, SendMessageOptions.RequireReceiver);
        }
    }
}
