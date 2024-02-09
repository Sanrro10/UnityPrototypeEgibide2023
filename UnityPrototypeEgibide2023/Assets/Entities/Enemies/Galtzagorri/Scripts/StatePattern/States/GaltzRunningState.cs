using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzRunningState : IState
    {
        // Referencia al script principal
        private NewGaltzScript _entity;
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public GaltzRunningState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            // Iniciar movimiento
            _entity.InvokeRepeating(nameof(_entity.Move), 0f, 0.01f);
            
            // Iniciar comprobación de donde está el player
            _entity.InvokeRepeating(nameof(_entity.FollowPlayer), 0f, 0.1f);
            
            // Iniciar animación "IsRunning"
            _entity.animator.SetBool(IsRunning,true);
        }

        public void Update()
        {
            _entity.target = _entity.playerGameObject.transform.position;
        }

        public void Exit()
        {
            // Terminar animación "IsRunning"
            _entity.animator.SetBool(IsRunning,false);
            
            // Cancelar comprobación de donde está el player
            _entity.CancelInvoke(nameof(_entity.FollowPlayer));
            
            // Cancelar movimiento
            _entity.CancelInvoke(nameof(_entity.Move));
        }
    }

}