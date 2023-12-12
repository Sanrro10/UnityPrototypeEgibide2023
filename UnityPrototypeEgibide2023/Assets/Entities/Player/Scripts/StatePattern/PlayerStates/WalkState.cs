
using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class WalkState : IState
    {
        private PlayerController player;
        
        public WalkState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            //Debug.Log("Entering Walk State");
            // Debug.Log("Entering Walk State");
            player.animator.SetBool("IsMoving", true);
            player.InvokeRepeating(nameof(player.Move), 0, 0.01f);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            

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

            if (!player.isHoldingHorizontal)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }
            
            if (player.CanDash())
            {
                player.PmStateMachine.TransitionTo((player.PmStateMachine.GroundDashState));
                return;
            }

            if (player.isPerformingMeleeAttack)
            {
                player.GroundAttack();
                return;
            }

        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Walk State");
            player.CancelInvoke(nameof(player.Move));
            player.animator.SetBool("IsMoving", false);
        }
    }
}