using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzHiddenState : IState
    {
        // Referencia al script principal
        private NewGaltzScript _entity;
        private static readonly int IsIdle1 = Animator.StringToHash("IsIdle1");

        public GaltzHiddenState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            // Poner por defecto que pueda salir del escondite
            _entity.canExit = true;
            
            // Quitarle la velocidad
            _entity.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            
            // Hacer que no pueda girar mientras está escondido
            _entity.CancelInvoke(nameof(_entity.CheckDirection));
            
            // Si justo se esconde mientras comprueba la dirección y tiene que girar, que gire muy rápido
            _entity.InvokeRepeating(nameof(_entity.CheckDirection), 0f, 0.01f);
            
            // Desactivar hitboxes
            _entity.AlternateHitbox(false);
            
            // Iniciar animación "IsIdle1" (Escondido)
            _entity.animator.SetBool(IsIdle1,true);
            
            // Iniciar la corutina para esperar 2 segundos
            _entity.StartCoroutine(nameof(_entity.Wait));
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            // Terminar animación "IdIdle1" (Escondido)
            _entity.animator.SetBool(IsIdle1, false);
            
            // Activar hitboxes
            _entity.AlternateHitbox(true);
            
            // Cancelar comprobación de girar
            _entity.CancelInvoke(nameof(_entity.CheckDirection));
            
            // Iniciar la comprobación para girar a la velocidad normal
            _entity.InvokeRepeating(nameof(_entity.CheckDirection), 0f, 0.03f);
            
            // Quitar el escondite actual
            _entity.currentHideout = null;
            
            // Cambiar la variable para que pueda salir
            _entity.canExit = true;
        }
    }

}