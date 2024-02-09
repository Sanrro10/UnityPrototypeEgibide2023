using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzAttackState : IState
    {
        // Referencia al script principal
        private NewGaltzScript _entity;
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        public GaltzAttackState(NewGaltzScript entity)
        {
            this._entity = entity;
        }
        
        public void Enter()
        {
            // Cancela el el Check Direction para que no gire mientras ataca
            _entity.CancelInvoke(nameof(_entity.CheckDirection));
            
            // Inicia la animación de "IsJumping" (atacar)
            _entity.animator.SetBool(IsJumping, true);
            
            // Activa la zona de ataque
            _entity.attackZone.SetActive(true);
            
            // Hace saltar el galtzagorri hacia el jugador
            _entity.GetComponent<Rigidbody2D>().AddForce(new Vector2((_entity.playerGameObject.transform.position.x - _entity.transform.position.x) * 2, 5), ForceMode2D.Impulse);
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            // Desactiva la zona de ataque
            _entity.attackZone.SetActive(false);
            
            // Quita la animación de "IsJumping" (atacar)
            _entity.animator.SetBool(IsJumping, false);
            
            // Vuelve a iniciar el CheckDirection
            _entity.InvokeRepeating(nameof(_entity.CheckDirection), 0f, 0.1f);
        }
    }

}
