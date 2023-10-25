
using UnityEngine;

namespace StatePattern.PlayerStates
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
            player.animator.SetBool("OnAir", true);
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {

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
                player.pmStateMachine.TransitionTo((player.pmStateMachine.DashState));
                return;
            }

            if (player.isMoving)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.AirMoveState);
                return;
            }

        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Air State");
            
            player.animator.SetBool("OnAir", false);
        }
    }
}