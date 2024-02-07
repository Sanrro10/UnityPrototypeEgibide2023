using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzDeathState : IState
    {
        private NewGaltzScript _entity;
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        public GaltzDeathState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            _entity.animator.SetBool(IsDead, true);
            _entity.AlternateHitbox(false);
            _entity.Die();
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }

}