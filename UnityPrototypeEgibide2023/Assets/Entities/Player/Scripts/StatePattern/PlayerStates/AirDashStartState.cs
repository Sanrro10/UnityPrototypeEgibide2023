using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirDashStartState : AirState
    {
        
        public AirDashStartState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            Player.isDashing = true;
            Player.animator.SetTrigger("Dash");

            Player.SetGravity(0);
            Player.SetXVelocity(0);
            Player.SetYVelocity(0);
            

            Player.StartCoroutine(Player.FloatDuration());
            Player.StartCoroutine(Player.AirDashCooldown());
            
            // Debug.Log("Entering AirDashStart State");
            // Debug.Log("Entering AirDashStart State");
            // Initialize Dash
        }


        
        public override void Exit()
        {   
            base.Exit();
        }
    }
}