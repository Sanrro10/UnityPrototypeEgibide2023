using System;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzHideoutRange : MonoBehaviour
    {
        // Evento cuando el player entra en el rango del escondite
        public static event Action<GameObject> PlayerEntered;
        
        // Evento cuando el player sale del rango del escondite
        public static event Action<GameObject> PlayerExited; 
        
        // Lanza el evento de que el player ha entrado
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerEntered?.Invoke(gameObject.transform.parent.gameObject);
            
        }

        // Lanza el evento de que el player ha salido
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerExited?.Invoke(gameObject.transform.parent.gameObject);
            
        }

        // Lanza el evento mientras el player siga dentro
        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerEntered?.Invoke(gameObject.transform.parent.gameObject);
        }
    }
}
