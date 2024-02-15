using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class MeleeAttackLeftState : AttackState
    {
        private List<AttackComponent.AttackData> _attackData;
        public MeleeAttackLeftState(PlayerController player) : base(player)
        {
            AttackDirection = new Vector2(1, 0.5f);
            KnockbackMultiplier = 1.5f;
        }

        public override void Enter()
        {
            Player.FacingRight = false;
            base.Enter();
            // Debug.Log("Entering Left Attack State");

            
            Player.isInMiddleOfAttack = true;
            Player.animator.SetBool("IsALeft", true);
            Player.Invoke(nameof(Player.EndAttack), 0.4f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();
            if (!Player.IsGrounded())
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.AirborneState);
                return;
            }
            if (Player.CanDash())
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.GroundDashState);
                return;
            }
            if (Player.isPerformingJump)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.JumpState);
                return;
            }
            if (!Player.isInMiddleOfAttack)
            {
                if(Player.isHoldingHorizontal) Player.PmStateMachine.TransitionTo(Player.PmStateMachine.WalkState);
                else Player.PmStateMachine.TransitionTo(Player.PmStateMachine.IdleState);
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.CancelInvoke(nameof(Player.EndAttack));
            Player.isInMiddleOfAttack = false;

            Player.animator.SetBool("IsALeft", false);
            
            // Debug.Log("Exit Left Attack State");
        }
    }
}
