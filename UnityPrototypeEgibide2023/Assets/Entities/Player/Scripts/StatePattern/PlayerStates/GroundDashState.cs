using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
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
            // Debug.Log("Entering Ground Dash State");
            
            player.isDashing = true;
            player.animator.SetBool("IsDash", true);
            player.FlipSprite();
            //player.StartCoroutine((player.DashCooldown()));
            player.StartCoroutine(player.Dash());
            player.Invulneravility();
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (!player.IsGrounded())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                return;
            }

            
            if (!player.isDashing)
            {
                if (player.isHoldingHorizontal)
                {
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.WalkState);
                    return;
                }
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
            }
            
            
            
        }
        
        public void Exit()
        {
            player.EndInvulneravility();
            player.animator.SetBool("IsDash", false);
            player.CancelInvoke(nameof(player.Dash));
            player.StartCoroutine(player.DashCooldown());
            player.isDashing = false;
            
            // Debug.Log("Exiting Dash State");
        }
    }
}