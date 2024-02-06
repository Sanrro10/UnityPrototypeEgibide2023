using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class MeleeAttackUpState : AttackState
    {
        
        public MeleeAttackUpState(PlayerController player): base(player)
        {
            AttackDirection = new Vector2(0, 1);
            KnockbackMultiplier = 1.5f;
        }

        public override void Enter()
        {
            base.Enter();
            // Debug.Log("Entering Up Attack State");
            
            
            Player.isInMiddleOfAttack = true;
            Player.animator.SetBool("IsAUp", true);
            Player.Invoke(nameof(Player.EndAttack), 0.3f);
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

            Player.animator.SetBool("IsAUp", false);
            
            // Debug.Log("Exit Up Attack State");
        }
    }
}
