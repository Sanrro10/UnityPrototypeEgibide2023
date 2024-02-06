using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzActiveZone : MonoBehaviour
    {
        [SerializeField] private GameObject[] galtzagorris;
        public bool isIn;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            isIn = true;
            foreach (var galtzagorri in galtzagorris)
            {
                var script = galtzagorri.GetComponent<NewGaltzScript>();
                if (script.StateMachine.CurrentState == script.StateMachine.GaltzHiddenState)
                {
                    script.StateMachine.TransitionTo(script.StateMachine.GaltzRunningState);
                }
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            isIn = false;
            foreach (var galtzagorri in galtzagorris)
            {
                var script = galtzagorri.GetComponent<NewGaltzScript>();
                if (script.StateMachine.CurrentState == script.StateMachine.GaltzRunningState ||
                    script.StateMachine.CurrentState == script.StateMachine.GaltzAttackState)
                {
                    script.StateMachine.TransitionTo(script.StateMachine.GaltzHidingState);
                }
            }
            
        }

        
    }
}
