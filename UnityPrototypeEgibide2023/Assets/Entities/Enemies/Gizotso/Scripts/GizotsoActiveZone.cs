using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class GizotsoActiveZone : MonoBehaviour
    {
        private bool _playerInside;
        // Evento que activa el ataque del Gizotso
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            _playerInside = true;
            var script = transform.parent.GetComponent<Gizotso>();
            script.PrepareAttack();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            _playerInside = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            var script = transform.parent.GetComponent<Gizotso>();
            script.PrepareAttack();
        }

        public bool PlayerInside => _playerInside;
    }
}
