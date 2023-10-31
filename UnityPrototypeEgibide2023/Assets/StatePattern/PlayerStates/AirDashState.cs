using UnityEngine;

namespace StatePattern.PlayerStates
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
            
            Debug.Log("Entering Air Dash State");
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

        }
    }
}