using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    public class VisionZone : MonoBehaviour
    {
        public GameObject consumer;
    

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            GoatBehaviour goatBehaviour = consumer.GetComponent<GoatBehaviour>();
            goatBehaviour.stateMachine.TransitionTo(goatBehaviour.stateMachine.GoatChargeState);
        }
    }
}
