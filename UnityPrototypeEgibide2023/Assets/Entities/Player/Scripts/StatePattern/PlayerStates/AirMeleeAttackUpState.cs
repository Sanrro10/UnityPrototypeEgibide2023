using Entities.Player.Scripts;
using UnityEngine;
namespace StatePattern.PlayerStates
{
    public class AirMeleeAttackUpState : IState
    {
        private PlayerController player;
        
        public AirMeleeAttackUpState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Air Up Attack State");

            player.isInMiddleOfAirAttack = true;
            player.animator.SetBool("IsAAUp", true);
            player.InvokeRepeating(nameof(player.AirMove), 0, 0.01f);
            player.Invoke(nameof(player.EndAirAttack), 0.8f);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {     
            if (!player.isInMiddleOfAirAttack)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                return;
            }
            if (player.IsGrounded()) {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }
        }
        
        public void Exit()
        {
            player.CancelInvoke(nameof(player.AirMove));
            player.CancelInvoke(nameof(player.EndAirAttack));
            player.animator.SetBool("IsAAUp", false);
            player.canAttack = true;
            player.isInMiddleOfAirAttack = false;

            Debug.Log("Exiting Air Up Attack State");
        }
    }
}
