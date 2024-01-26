using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class IdleState: IState
    {
        private PlayerController player;
        
        public IdleState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // Debug.Log("Entering Idle State");
            player.setXVelocity(0);
            player.animator.SetBool("IsIdle", true);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            if (player.isHoldingHorizontal)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.WalkState);
                return;
            }

            if (player.CanDash())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.GroundDashState);
                return;
            }

            if (player.isPerformingJump)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.JumpState);
                return;
            }

            if (!player.IsGrounded())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                
                return;
            }
            
            if (player.isPerformingMeleeAttack)
            {
                player.GroundAttack();
                return;
            }
            
            if (player.CanThrowPotion())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.ThrowPotionState);
                return;
            }

            
        }
        
        public void Exit()
        {
            player.animator.SetBool("IsIdle", false);
            // Debug.Log("Exiting Idle State");
        }
    }
}