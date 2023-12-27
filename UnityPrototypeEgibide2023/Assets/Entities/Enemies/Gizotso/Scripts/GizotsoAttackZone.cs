using System.Collections;
using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class GizotsoAttackZone : MonoBehaviour
    {
        public void Attack()
        {
            StartCoroutine(nameof(Cooldown));
        }

        // Corutina que desactiva la hitbox del ataque después de X segundos
        private IEnumerator Cooldown()
        {
            // TODO: Este es el tiempo de la animacion en la que va a estar activa la hitbox de ataque
            yield return new WaitForSeconds(0.3f);
            gameObject.SetActive(false);
        }
    
        // Evento del Collider para que el jugador reciba daño
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            other.GetComponentInParent<PlayerController>().OnReceiveDamage(1);
        }
    }
}
