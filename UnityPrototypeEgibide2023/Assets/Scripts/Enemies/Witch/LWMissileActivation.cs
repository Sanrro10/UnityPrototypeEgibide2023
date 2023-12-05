using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LWMissileActivation : MonoBehaviour
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
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>() != null)
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("setMissilePossible", true, SendMessageOptions.RequireReceiver);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>() != null)
        {
            gameObject.GetComponentInParent<LandWitch>().SendMessage("setMissilePossible", false, SendMessageOptions.RequireReceiver);
        }
    }
}