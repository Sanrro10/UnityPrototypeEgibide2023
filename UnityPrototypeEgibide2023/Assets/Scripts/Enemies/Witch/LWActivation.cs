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
        Debug.Log("Ha entrado algo en mi radio de accion JIJIJI");
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Cuidado, es el heroe");
            gameObject.GetComponentInParent<LandWitch>().SendMessage("setActiveState" , true, SendMessageOptions.RequireReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>() != null)
        {
            //End the timer that throws potions, but the tp one, once activated, will follow the player periodically
            Debug.Log("Sale el heroe");
            gameObject.GetComponentInParent<LandWitch>().SendMessage("setActiveState" , false , SendMessageOptions.RequireReceiver);
            
        }
    }
}
