using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzHidingState : IState
    {
        private NewGaltzScript _entity;
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public GaltzHidingState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            _entity.PlaceToHide(null);
            _entity.InvokeRepeating(nameof(_entity.Move), 0f, 0.01f);
            _entity.animator.SetBool(IsRunning,true);
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _entity.animator.SetBool(IsRunning,false);
            _entity.CancelInvoke(nameof(_entity.Move));
        }
    }

}