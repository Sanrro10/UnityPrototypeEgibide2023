using System.Collections;
using System.Collections.Generic;
using Entities.Player.Scripts;
using UnityEngine;

public class AirdashUnlocker : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc == null) return;
            pc.isAirDashUnlocked = true;
        }
    }
}
