using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzDeathState : IState
    {
        // Referencia al script principal
        private NewGaltzScript _entity;
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        public GaltzDeathState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            // Iniciar animación "IsDead"
            _entity.animator.SetBool(IsDead, true);
            
            // Cancelar comprobación del giro
            _entity.CancelInvoke(nameof(_entity.CheckDirection));
            
            // Hacer que caiga al suelo
            _entity.GetComponent<Rigidbody2D>().GetComponent<Rigidbody2D>().mass = 500;

            // Si está en el suelo hacer directamente la lógica de muerte
            if (_entity.isGrounded)
            {
                _entity.Die();
            }
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }

}