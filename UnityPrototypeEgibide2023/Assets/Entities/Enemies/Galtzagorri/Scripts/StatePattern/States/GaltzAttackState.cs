using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzAttackState : IState
    {
        private NewGaltzScript _entity;
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");

        public GaltzAttackState(NewGaltzScript entity)
        {
            this._entity = entity;
        }
        
        public void Enter()
        {
            _entity.CancelInvoke(nameof(_entity.CheckDirection));
            _entity.animator.SetBool(IsJumping, true);
            _entity.attackZone.SetActive(true);
            //_entity.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            _entity.GetComponent<Rigidbody2D>().AddForce(new Vector2((_entity.playerGameObject.transform.position.x - _entity.transform.position.x) * 2, 5), ForceMode2D.Impulse);
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _entity.attackZone.SetActive(false);
            _entity.animator.SetBool(IsJumping, false);
            _entity.InvokeRepeating(nameof(_entity.CheckDirection), 0f, 0.1f);
        }
    }

}
