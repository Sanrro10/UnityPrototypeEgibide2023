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
            // Iniciar la animación de "IsDead" (muerte)
            _entity.animator.SetBool(IsDead, true);
            
            // Desactivar hitboxes
            _entity.AlternateHitbox(false);
            
            // Llamar a la lógica de muerte
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