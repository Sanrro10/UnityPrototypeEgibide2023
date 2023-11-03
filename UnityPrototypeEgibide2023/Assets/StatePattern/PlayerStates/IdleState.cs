
using UnityEngine;

namespace StatePattern.PlayerStates
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
            //Debug.Log("Entering Idle State");
            player.SetXVelocity(0);
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

            if (player.isJumping)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.JumpState);
                return;
            }

            if (!player.IsGrounded())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                
                return;
            }
            
            
            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            if (player.isPerformingMeleeAttack)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.MeleeAttackState);
                return;
            }
            
            // if we press the dash button, transition to the dash state
            
            
            
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Idle State");
        }
    }
}