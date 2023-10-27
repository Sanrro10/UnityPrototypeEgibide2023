using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AirDashStartState : IState
    {
        private PlayerController player;
        
        public AirDashStartState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.onDashCooldown = true;
            player.isDashing = true;
            player.SetCurrentGravity(0);
            
            player.animator.SetTrigger("Dash");
            
            //player.Dash();
            player.StartCoroutine(player.FloatDuration());
            
            Debug.Log("Entering AirDashStart State");
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

        }
    }
}