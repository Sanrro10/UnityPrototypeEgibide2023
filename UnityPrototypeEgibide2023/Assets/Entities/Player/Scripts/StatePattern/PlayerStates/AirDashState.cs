using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirDashState : IState
    {
        private PlayerController player;
        
        public AirDashState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {

            player.isDashing = true;
            //player.animator.SetTrigger("Dash");
            player.AirDash();
            player.StartCoroutine(player.AirDashDuration());
            player.StartCoroutine(player.AirDashCooldown());
            // Debug.Log("Entering Air Dash State");
            // Initialize Dash
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            if (player.IsGrounded())
            {
                if (player.isHoldingHorizontal)
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.WalkState);
                else
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
            }
            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state
            
            
            
        }
        
        public void Exit()
        {
            player.StopCoroutine(player.AirDashDuration());
            player.isDashing = false;

        }
    }
}