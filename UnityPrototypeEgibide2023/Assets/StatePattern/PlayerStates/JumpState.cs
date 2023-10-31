
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class JumpState : IState
    {
        private PlayerController player;
        
        public JumpState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            
            Debug.Log("Entering Jump State");
            player.animator.SetTrigger("Jump");
            player.Jump();
            player.StartCoroutine(player.GroundedCooldown());
            player.pmStateMachine.TransitionTo(player.pmStateMachine.AirState);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state
            if (player.isDashing)
            {
                player.pmStateMachine.TransitionTo((player.pmStateMachine.GroundDashState));
                return;
            }
            
            
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Jump State");
            
            player.isJumping = false;
        }
    }
}