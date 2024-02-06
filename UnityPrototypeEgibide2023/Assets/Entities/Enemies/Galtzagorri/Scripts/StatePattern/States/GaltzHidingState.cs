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
            var placeToHide = Random.Range(0, _entity.hideouts.Length);
            if (placeToHide < 0 || placeToHide >= _entity.hideouts.Length) return;
            _entity.currentHideout = _entity.hideouts[placeToHide];
            _entity.target = _entity.hideouts[placeToHide].transform.position;
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