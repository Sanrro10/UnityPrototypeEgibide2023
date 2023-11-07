
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
            player.InvokeRepeating(nameof(player.Dash), 0, 0.01f);
            player.StartCoroutine(player.DashDuration());
            // Initialize Dash
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            if (!player.CanDash())
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
            Debug.Log("Exit Dash State");

        }
    }
}