using System.Collections.Generic;
using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AttackState : IState
    {
        protected PlayerController _player;
        protected Vector2 _attackDirection;
        protected float _knockbackMultiplier;
        public AttackState(PlayerController player)
        {
            this._player = player;
        }
        
        public virtual void Enter()
        {
            _player.meleeAttack.GetComponent<AttackComponent>().AddAttackData(new AttackComponent.AttackData(_player.GetPlayerData().damage, _player.GetPlayerData().knockback * _knockbackMultiplier, _attackDirection, 6));
            _player.meleeAttack.GetComponent<AttackComponent>().AddAttackData(new AttackComponent.AttackData(1, _player.GetPlayerData().knockback * _knockbackMultiplier, _attackDirection, 7));
        }

        public virtual void Update()
        {
            
        }

        public virtual void Exit()
        {
            _player.meleeAttack.GetComponent<AttackComponent>().ClearAttackData();
        }
    }
}