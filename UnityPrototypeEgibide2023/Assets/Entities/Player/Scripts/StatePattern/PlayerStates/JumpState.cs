using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class JumpState : IState
    {
        private PlayerController player;
        
        public JumpState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Jump State");
            player.animator.SetBool("IsJump", true);
            player.InvokeRepeating(nameof(player.Jump), 0, 0.01f);
            player.StartCoroutine(player.MaxJumpDuration());
            player.StartCoroutine(player.GroundedCooldown());
            player.InvokeRepeating(nameof(player.AirMove), 0, 0.01f);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {

            if (!player.isPerformingJump)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                return;
            }
            
            if (player.CanAirDash())
            {
                player.PmStateMachine.TransitionTo((player.PmStateMachine.AirDashStartState));
                return;
            }
            if (player.isPerformingMeleeAttack)
            {
                player.AirAttack();
            }
            
            if (player.isHoldingHorizontal)
            {
                player.AirMove();
                return;
            }
            
        }
        
        public void Exit()
        {
            player.animator.SetBool("IsJump", false);
            player.CancelInvoke(nameof(player.AirMove));
            player.CancelInvoke(nameof(player.Jump));
            player.isPerformingJump = false;
            
            Debug.Log("Exiting Jump State");
        }
    }
}