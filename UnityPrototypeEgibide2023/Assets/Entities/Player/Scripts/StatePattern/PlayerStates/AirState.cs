using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirState: IState
    {
        private PlayerController player;
        public AirState(PlayerController player)
        {
            this.player = player;
        }
        
        public void Enter()
        {
            Debug.Log("Entering Air State");

            player.animator.SetBool("IsAir", true);
            player.InvokeRepeating(nameof(player.AirMove), 0, 0.01f);
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {

            if (player.IsGrounded())
            {
                if(player.isHoldingHorizontal)
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.WalkState);
                else
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }
            
            if (player.isPerformingMeleeAttack)
            {
                player.AirAttack();
                return;
            }

            
            if (player.CanAirDash())
            {
                player.PmStateMachine.TransitionTo((player.PmStateMachine.AirDashStartState));
                return;
            }

        }
        
        public void Exit()
        {
            Debug.Log("Exiting Air State");
            player.CancelInvoke(nameof(player.AirMove));
            player.animator.SetBool("IsAir", false);
        }
    }
}