using UnityEngine;

namespace Entities.Enemies.Goat
{
    public class VisionZone : MonoBehaviour
    {
        public GameObject consumer;
    

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            consumer.GetComponent<GoatBehaviour>().ActivateEnemy();
        }
    }
}
