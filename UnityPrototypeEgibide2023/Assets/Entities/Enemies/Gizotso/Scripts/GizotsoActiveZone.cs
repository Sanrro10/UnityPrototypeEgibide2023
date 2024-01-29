using System;
using UnityEngine;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class GizotsoActiveZone : MonoBehaviour
    {
        private bool _playerInside;
        // Evento que activa el ataque del Gizotso
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            _playerInside = true;
            var script = transform.parent.GetComponent<Gizotso>();
            script.Attack();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            _playerInside = false;
        }
        
        public bool PlayerInside => _playerInside;
    }
}
