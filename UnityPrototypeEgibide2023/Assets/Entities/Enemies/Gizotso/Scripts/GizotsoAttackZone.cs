using System.Collections;
using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class GizotsoAttackZone : MonoBehaviour
    {
    
        // Evento del Collider para que el jugador reciba daño
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            other.GetComponentInParent<PlayerController>().OnReceiveDamage(1);
        }
    }
}
