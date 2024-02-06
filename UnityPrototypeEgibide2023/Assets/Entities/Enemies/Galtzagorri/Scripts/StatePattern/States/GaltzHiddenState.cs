using StatePattern;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern.States
{
    public class GaltzHiddenState : IState
    {
        private NewGaltzScript _entity;
        private static readonly int IsIdle1 = Animator.StringToHash("IsIdle1");

        public GaltzHiddenState(NewGaltzScript entity)
        {
            this._entity = entity;
        }

        public void Enter()
        {
            _entity.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            _entity.AlternateHitbox(false);
            _entity.animator.SetBool(IsIdle1,true);
            _entity.StartCoroutine(nameof(_entity.Wait));
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            _entity.animator.SetBool(IsIdle1, false);
            _entity.AlternateHitbox(true);
        }
    }

}