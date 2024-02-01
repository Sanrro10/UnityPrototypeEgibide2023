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
            Player.Invulneravility();
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();
            
            if (Player.isPerformingJump) {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.JumpState);
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
            Player.EndInvulneravility();
            Player.animator.SetBool("IsDash", false);
            Player.StartCoroutine(Player.DashCooldown());
            Player.isDashing = false;
            Player.GetRigidbody().velocity = new Vector2((Player.FacingRight ? 20f  : -20f), Player.GetRigidbody().velocity.y);

            // Debug.Log("Exiting Dash State");
        }
    }
}