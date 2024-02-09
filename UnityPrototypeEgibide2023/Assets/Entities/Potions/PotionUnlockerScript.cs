using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionUnlockerScript : MonoBehaviour
{

    public static event Action<GameObject> OnPotionUnlock;//Evento a escuchar para desbloquear pociones

    public GameObject potionToUnlock;//La pocion que se va a desbloquear

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPotionUnlock?.Invoke(potionToUnlock);
            Destroy(gameObject);
        }
    }
    
    
}
