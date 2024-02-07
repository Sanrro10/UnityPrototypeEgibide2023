using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class GaltzActiveZone : MonoBehaviour
    {
        [SerializeField] public GameObject[] galtzagorris;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            foreach (var galtzagorri in galtzagorris)
            {
                if (galtzagorri is not null)
                {
                    var script = galtzagorri.GetComponent<NewGaltzScript>();
                    script.isIn = true;
                    if (script.canExit)
                    {
                        if (script.StateMachine.CurrentState == script.StateMachine.GaltzHiddenState)
                        {
                            script.StateMachine.TransitionTo(script.StateMachine.GaltzRunningState);
                        }
                    }
                }
                
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("PlayerHelper")) return;
            if (other.gameObject.name != "EnemyDetection") return;
            
            foreach (var galtzagorri in galtzagorris)
            {
                if (galtzagorri is not null)
                {
                    var script = galtzagorri.GetComponent<NewGaltzScript>();
                    script.isIn = false;
                    if (script.StateMachine.CurrentState == script.StateMachine.GaltzRunningState ||
                        script.StateMachine.CurrentState == script.StateMachine.GaltzAttackState)
                    {
                        script.StateMachine.TransitionTo(script.StateMachine.GaltzHidingState);
                    }
                }
            }
            
        }

        
    }
}
