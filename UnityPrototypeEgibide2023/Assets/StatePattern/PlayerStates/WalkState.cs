
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
            Debug.Log("Entering Walk State");
            player.animator.SetBool("IsMoving", true);
            player.InvokeRepeating(nameof(player.Move), 0, 0.01f);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            

            if (player.isJumping)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.JumpState);
                return;
            }

            if (!player.isMoving)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.IdleState);
                return;
            }
            
            if (player.isDashing)
            {
                player.pmStateMachine.TransitionTo((player.pmStateMachine.DashState));
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