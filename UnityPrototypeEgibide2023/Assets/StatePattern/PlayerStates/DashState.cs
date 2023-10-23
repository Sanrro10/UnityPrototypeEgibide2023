
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class DashState : IState
    {
        private PlayerController player;
        
        public DashState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.onDashCooldown = true;
            player.isDashing = true;
            player.animator.SetTrigger("Dash");
            player.Dash();
            player.StartCoroutine(player.DashDuration());
            
            Debug.Log("Entering Dash State");
            // Initialize Dash
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            
            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state
            
            
            
        }
        
        public void Exit()
        {
            player.isDashing = false;
            player.onDashCooldown = false;
        }
    }
}