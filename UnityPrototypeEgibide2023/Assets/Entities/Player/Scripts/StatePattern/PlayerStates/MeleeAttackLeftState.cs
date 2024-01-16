using System.Collections.Generic;
using Entities.Player.Scripts;
using UnityEngine;
namespace StatePattern.PlayerStates
{
    public class MeleeAttackLeftState : AttackState
    {
        private List<AttackComponent.AttackData> _attackData;
        public MeleeAttackLeftState(PlayerController player) : base(player)
        {
            _player = player;
            _attackDirection = new Vector2((_player.facingRight ? -1 : 1), 0);
            _knockbackMultiplier = 1.5f;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Entering Left Attack State");
            
            
            _player.isInMiddleOfAttack = true;
            _player.animator.SetBool("IsALeft", true);
            _player.Invoke(nameof(_player.EndAttack), 0.8f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            if (!_player.isInMiddleOfAttack)
            {
                _player.PmStateMachine.TransitionTo(_player.PmStateMachine.IdleState);
                return;
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            _player.CancelInvoke(nameof(_player.EndAttack));
            _player.canAttack = true;
            _player.isInMiddleOfAttack = false;

            _player.animator.SetBool("IsALeft", false);
            
            Debug.Log("Exit Left Attack State");
        }
    }
}
