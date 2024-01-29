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
            //player.animator.SetTrigger("Dash");
            player.AirDash();
            player.StartCoroutine(player.AirDashDuration());
            player.StartCoroutine(player.DashCooldown());
            // Debug.Log("Entering Air Dash State");
            // Initialize Dash
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (player.IsGrounded())
            {
                if (player.isHoldingHorizontal)
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.WalkState);
                else 
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
            }
        }
        
        public void Exit()
        {
            player.StopCoroutine(player.AirDashDuration());
            

        }
    }
}