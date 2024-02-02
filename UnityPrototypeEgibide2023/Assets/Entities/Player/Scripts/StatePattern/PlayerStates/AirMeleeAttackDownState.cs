using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirMeleeAttackDownState : AttackState
    {
        
        public AirMeleeAttackDownState(PlayerController player) : base(player)
        {
            AttackDirection = new Vector2(0, -1);
            KnockbackMultiplier = 1.5f;
            
        }

        public override void Enter()
        {
            base.Enter();
            Player.isInMiddleOfAirAttack = true;
            Player.animator.SetBool("IsAADown", true);
            Player.InvokeRepeating(nameof(Player.AirMove), 0, 0.01f);
            Player.Invoke(nameof(Player.EndAirAttack), 0.5f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {     
            base.Update();
            if (!Player.isInMiddleOfAirAttack)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.AirborneState);
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
            Player.animator.SetBool("IsAADown", false);
            Player.isInMiddleOfAirAttack = false;

            // Debug.Log("Exiting Air Down Attack State");
        }
    }
}
