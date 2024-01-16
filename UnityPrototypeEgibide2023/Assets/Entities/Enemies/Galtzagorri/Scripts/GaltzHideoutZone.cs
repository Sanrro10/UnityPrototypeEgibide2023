using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class galtzHideoutZone : MonoBehaviour
    {
        // Variable que controla si el enemigo te puede hacer daño
        private bool _enemyHit;
    
        // Evento para controlar si el enemigo se choca con el Player o si se choca con un escondite
        private void OnTriggerEnter2D(Collider2D other)
        {
            GameObject parent = transform.parent.gameObject;
            var script = parent.GetComponent<GaltzScript>();
            if (other.gameObject.CompareTag("Player") && !_enemyHit) {
                other.GetComponentInParent<PlayerController>().OnReceiveDamage(1, 0, Vector2.zero);
                _enemyHit = true;
            }

            if (other.gameObject.CompareTag("EnemySpawnPoint"))
            {
                script.ActivateHiding(other);
            }
                
        }

        // Metodo para resetear que el enemigo te pueda hacer daño
        public void ResetHit()
        {
            _enemyHit = false;
        }
    }
}
