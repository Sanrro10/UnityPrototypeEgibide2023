
using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AirMoveState : IState
    {
        private PlayerController player;
        
        public AirMoveState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // Debug.Log("Entering DJump State");
            player.InvokeRepeating(nameof(player.Move), 0, 0.01f);
            Debug.Log("Entering Air Move State");

        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state

            
            if (player.IsGrounded())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }
            if (player.isPerformingJump && !player.onDJump)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.DJumpState);
                return;
            }
            if (player.isPerformingDash && player.isAirDashUnlocked)
            {
                player.PmStateMachine.TransitionTo((player.PmStateMachine.AirDashStartState));
                return;
            }
            
            if (!player.isHoldingHorizontal)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                return;
            }

            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state
            
            
            
        }
        
        public void Exit()
        {
            player.CancelInvoke(nameof(player.Move));
            // Debug.Log("Exiting DJump State");
        }
    }
}