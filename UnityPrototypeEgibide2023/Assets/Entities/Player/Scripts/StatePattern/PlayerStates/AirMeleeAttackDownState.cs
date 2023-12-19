using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AirMeleeAttackDownState : IState
    {
        private PlayerController player;
        
        public AirMeleeAttackDownState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Air Down Attack State");

            player.isInMiddleOfAirAttack = true;
            player.animator.SetBool("IsAADown", true);
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
            player.animator.SetBool("IsAADown", false);
            player.canAttack = true;
            player.isInMiddleOfAirAttack = false;

            Debug.Log("Exiting Air Down Attack State");
        }
    }
}
