using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirMeleeAttackBackwardState : AttackState
    {
        
        public AirMeleeAttackBackwardState(PlayerController player) : base(player)
        {
            AttackDirection = new Vector2(-1, 0.5f);
            KnockbackMultiplier = 2.5f;
        }

        public override void Enter()
        {
            base.Enter();

            Player.isInMiddleOfAirAttack = true;
            if (Player.FacingRight) Player.animator.SetBool("IsAABackwardRight", true);
            else Player.animator.SetBool("IsAABackwardLeft", true);
            Player.InvokeRepeating(nameof(Player.AirMove), 0, 0.01f);
            Player.Invoke(nameof(Player.EndAirAttack), 0.8f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {   
            base.Update();
            if (!Player.isInMiddleOfAirAttack)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.AirState);
                return;
            }
            if (Player.IsGrounded()) {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.IdleState);
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.CancelInvoke(nameof(Player.AirMove));
            Player.CancelInvoke(nameof(Player.EndAirAttack));
            Player.animator.SetBool("IsAABackwardRight", false);
            Player.animator.SetBool("IsAABackwardLeft", false);
            Player.canAttack = true;
            Player.isInMiddleOfAirAttack = false;
        }
    }
}
