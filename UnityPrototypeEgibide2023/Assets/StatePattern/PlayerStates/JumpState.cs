
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
            
            //Debug.Log("Entering Jump State");
            // Debug.Log("Entering Jump State");
            player.animator.SetTrigger("Jump");
            player.InvokeRepeating(nameof(player.Jump), 0, 0.01f);
            player.StartCoroutine(player.MaxJumpDuration());
            player.StartCoroutine(player.GroundedCooldown());
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state

            if (!player.isJumping)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                return;
            }
            
            if (player.isDashing)
            {
                player.PmStateMachine.TransitionTo((player.PmStateMachine.AirDashStartState));
                return;
            }
            if (player.isPerformingMeleeAttack)
            {
                player.AirAttack();
            }
            
            if (player.isHoldingHorizontal)
            {
                player.AirMove();
                return;
            }
            
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Jump State");
            player.CancelInvoke(nameof(player.Jump));
            player.isJumping = false;
        }
    }
}