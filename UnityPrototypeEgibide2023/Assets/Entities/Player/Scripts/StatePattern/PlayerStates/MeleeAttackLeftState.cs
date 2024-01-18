using System.Collections.Generic;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class MeleeAttackLeftState : AttackState
    {
        private List<AttackComponent.AttackData> _attackData;
        public MeleeAttackLeftState(PlayerController player) : base(player)
        {
            AttackDirection = new Vector2(-1, 0.2f);
            KnockbackMultiplier = 1.5f;
        }

        public override void Enter()
        {
            base.Enter();
            // Debug.Log("Entering Left Attack State");
            
            Player.isInMiddleOfAttack = true;
            Player.animator.SetBool("IsALeft", true);
            Player.Invoke(nameof(Player.EndAttack), 0.8f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();
            if (!Player.isInMiddleOfAttack)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.IdleState);
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            Player.CancelInvoke(nameof(Player.EndAttack));
            Player.canAttack = true;
            Player.isInMiddleOfAttack = false;

            Player.animator.SetBool("IsALeft", false);
            
            // Debug.Log("Exit Left Attack State");
        }
    }
}
