using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzActiveZone : MonoBehaviour
    {
        // Evento cuando el player entra en el area de activación
        public static event Action<GameObject> PlayerEnteredArea;
        
        // Evento cuando el player sale del area de activación
        public static event Action<GameObject> PlayerExitedArea;
        
        // Llama al evento cuando el player entra en el collider
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerEnteredArea?.Invoke(gameObject);
        }
        
        // Llama al evento cuando el player sale del collider
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerExitedArea?.Invoke(gameObject);
        }

        
    }
}
