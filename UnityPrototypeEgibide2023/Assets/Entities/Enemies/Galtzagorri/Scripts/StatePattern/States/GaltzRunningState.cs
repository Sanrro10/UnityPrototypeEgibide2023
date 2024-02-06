using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzRunningState : IState
    {
        private NewGaltzScript _entity;
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public GaltzRunningState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            _entity.InvokeRepeating(nameof(_entity.Move), 0f, 0.01f);
            _entity.InvokeRepeating(nameof(_entity.FollowPlayer), 0f, 0.1f);
            _entity.animator.SetBool(IsRunning,true);
        }

        public void Update()
        {
            _entity.target = _entity.playerGameObject.transform.position;
        }

        public void Exit()
        {
            _entity.animator.SetBool(IsRunning,false);
            _entity.CancelInvoke(nameof(_entity.FollowPlayer));
            _entity.CancelInvoke(nameof(_entity.Move));
        }
    }

}