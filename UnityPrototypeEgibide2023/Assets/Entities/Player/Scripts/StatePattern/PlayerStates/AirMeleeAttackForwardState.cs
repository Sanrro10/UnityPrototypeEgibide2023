using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirMeleeAttackForwardState : AttackState
    {
        public AirMeleeAttackForwardState(PlayerController player) : base(player)
        {
            AttackDirection = new Vector2(1, 0.5f);
            KnockbackMultiplier = 1.5f;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entering Air Forward Attack State");

            Player.isInMiddleOfAirAttack = true;
            if (Player.FacingRight) Player.animator.SetBool("IsAAForwardRight", true);
            else Player.animator.SetBool("IsAAForwardLeft", true);
            Player.animator.SetBool("IsAARight", true);
            Player.InvokeRepeating(nameof(Player.AirMove), 0, 0.01f);
            Player.Invoke(nameof(Player.EndAirAttack), 0.5f);
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
            Player.animator.SetBool("IsAAForwardRight", false);
            Player.animator.SetBool("IsAAForwardLeft", false);
            Player.isInMiddleOfAirAttack = false;

            Debug.Log("Entering Air Forward Attack State");
        }
    }
}
