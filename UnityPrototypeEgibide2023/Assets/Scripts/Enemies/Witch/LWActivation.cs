using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LWActivation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("SetActiveState" , true, SendMessageOptions.RequireReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //End the timer that throws potions, but the tp one, once activated, will follow the player periodically
            gameObject.GetComponentInParent<LandWitch>().SendMessage("SetActiveState" , false , SendMessageOptions.RequireReceiver);
            
        }
    }
}
