using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzHidingState : IState
    {
        // Referencia al script principal
        private NewGaltzScript _entity;
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");

        public GaltzHidingState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            // Seleccionar uno de los escondites
            _entity.PlaceToHide(null);
            
            // Iniciar el movimiento
            _entity.InvokeRepeating(nameof(_entity.Move), 0f, 0.01f);
            
            // Iniciar animación de "IsRunning"
            _entity.animator.SetBool(IsRunning,true);
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            // Terminar animación de "IsRunning"
            _entity.animator.SetBool(IsRunning,false);
            
            // Cancelar el movimiento
            _entity.CancelInvoke(nameof(_entity.Move));
        }
    }

}