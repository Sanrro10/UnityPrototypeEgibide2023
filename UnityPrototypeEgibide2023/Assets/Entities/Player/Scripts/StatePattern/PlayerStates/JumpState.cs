using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class JumpState : AirState
    {
        
        public JumpState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            // Debug.Log("Entering Jump State");
            base.Enter();
            Player.animator.SetBool("IsJump", true);
            Player.InvokeRepeating(nameof(Player.Jump), 0, 0.01f);
            Player.StartCoroutine(Player.MaxJumpDuration());
            Player.StartCoroutine(Player.GroundedCooldown());
            Player.InvokeRepeating(nameof(Player.AirMove), 0, 0.01f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();

            if (!Player.isPerformingJump)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.AirborneState);
                return;
            }
            
            if (Player.CanAirDash())
            {
                Player.PmStateMachine.TransitionTo((Player.PmStateMachine.AirDashStartState));
                return;
            }
            if (Player.isPerformingMeleeAttack)
            {
                Player.AirAttack();
            }
            
            if (Player.CanThrowPotion())
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.AirThrowPotionState);
                return;
            }
            
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.animator.SetBool("IsJump", false);
            Player.CancelInvoke(nameof(Player.AirMove));
            Player.CancelInvoke(nameof(Player.Jump));
            Player.isPerformingJump = false;
            
            // Debug.Log("Exiting Jump State");
        }
    }
}