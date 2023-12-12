using Entities.Player.Scripts;
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

            player.isDashing = true;
            
            
            player.animator.SetTrigger("Dash");

            player.SetGravity(0);
            player.SetXVelocity(0);
            player.SetYVelocity(0);
            

            player.StartCoroutine(player.FloatDuration());
            
            //Debug.Log("Entering AirDashStart State");
            // Debug.Log("Entering AirDashStart State");
            // Initialize Dash
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            player.FlipSprite();
    
            // if we press the jump button, transition to the jump state

            // if we press the attack button, transition to the attack state

            // if we press the dash button, transition to the dash state



        }
        
        public void Exit()
        {
            player.RestartGravity();
        }
    }
}