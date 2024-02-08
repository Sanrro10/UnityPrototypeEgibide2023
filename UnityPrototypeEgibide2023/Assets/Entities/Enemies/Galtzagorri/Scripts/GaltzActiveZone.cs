using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzActiveZone : MonoBehaviour
    {
        public static event Action<GameObject> PlayerEnteredArea;
        public static event Action<GameObject> PlayerExitedArea;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerEnteredArea?.Invoke(gameObject);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            PlayerExitedArea?.Invoke(gameObject);
        }

        
    }
}
