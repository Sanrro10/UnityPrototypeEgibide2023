using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizotsoActiveZone : MonoBehaviour
{
    
    // Evento que activa el ataque del Gizotso
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var script = transform.parent.GetComponent<Gizotso>();
        script.Attack();
    }
}
