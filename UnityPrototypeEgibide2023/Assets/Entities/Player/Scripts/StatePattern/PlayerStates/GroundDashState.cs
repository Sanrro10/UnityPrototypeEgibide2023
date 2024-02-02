using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class GroundDashState : GroundState
    {
        
        public GroundDashState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            Player.isDashing = true;
            Player.animator.SetBool("IsDash", true);
            Player.FlipSprite();
            //Player.StartCoroutine((Player.DashCooldown()));
            Player.StartCoroutine(Player.Dash());
            Player.Invulnerability();
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();
            
            if (Player.isPerformingJump) {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.JumpState);
                Player.Invoke(nameof(Player.SetOutOfDashVelocity), 0.05f);
                return;
            }
            
            if (!Player.isDashing)
            {
                if (Player.isHoldingHorizontal)
                {
                    Player.PmStateMachine.TransitionTo(Player.PmStateMachine.WalkState);
                    return;
                }
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.IdleState);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.StopCoroutine(nameof(Player.Dash));
            Player.EndInvulnerability();
            Player.animator.SetBool("IsDash", false);
            Player.StartCoroutine(Player.DashCooldown());
            Player.isDashing = false;
            // Debug.Log("Exiting Dash State");
        }
    }
}