
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
                player.pmStateMachine.TransitionTo(player.pmStateMachine.IdleState);
                return;
            }
            if (player.isJumping && !player.onDJump)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.DJumpState);
                return;
            }
            if (player.isDashing)
            {
                player.pmStateMachine.TransitionTo((player.pmStateMachine.AirDashStartState));
                return;
            }
            
            if (!player.isMoving)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.AirState);
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