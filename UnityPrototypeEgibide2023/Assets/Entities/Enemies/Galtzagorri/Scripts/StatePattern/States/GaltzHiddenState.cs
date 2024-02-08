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
            _entity.canExit = true;
            _entity.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            _entity.CancelInvoke(nameof(_entity.CheckDirection));
            _entity.AlternateHitbox(false);
            _entity.animator.SetBool(IsIdle1,true);
            _entity.StartCoroutine(nameof(_entity.Wait));
        }

        public void Update()
        {
            if (_entity.canExit && _entity.isIn && !_entity.waiting)
            {
                _entity.StartCoroutine(nameof(_entity.Wait));
            }
        }

        public void Exit()
        {
            _entity.animator.SetBool(IsIdle1, false);
            _entity.AlternateHitbox(true);
            _entity.InvokeRepeating(nameof(_entity.CheckDirection), 0f, 0.03f);
            _entity.currentHideout = null;
        }
    }

}