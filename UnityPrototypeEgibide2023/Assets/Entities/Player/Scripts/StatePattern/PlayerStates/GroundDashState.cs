
using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class GroundDashState : IState
    {
        private PlayerController player;
        
        public GroundDashState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.isDashing = true;
            player.animator.SetTrigger("Dash");
            player.FlipSprite();
            player.StartCoroutine((player.GroundedDashCooldown()));
            
            //Debug.Log("Entering Dash State");
            player.StartCoroutine(player.Dash());
            // Initialize Dash
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            if (!player.IsGrounded())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                return;
            }

            if (!player.isDashing)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
            }
            

            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state
            
            
            
        }
        
        public void Exit()
        {
            player.CancelInvoke(nameof(player.Dash));
            player.StartCoroutine(player.GroundedDashCooldown());
            player.isDashing = false;
            // Debug.Log("Exit Dash State");

        }
    }
}