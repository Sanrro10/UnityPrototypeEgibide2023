using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;


namespace Entities.Enemies.Goat.Scripts.StatePattern.States
{
    public class GoatPrepareState : IState
    {
        // Start is called before the first frame update
        private GoatBehaviour entity;
        
        public GoatPrepareState(GoatBehaviour entity)
        {
            this.entity = entity;
        }

        public void Enter()
        {
            entity.animator.SetBool("IsPrepare", true);
            entity.Invoke(nameof(entity.Charge), entity.data.waitTime);
        
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
        
        }
        
        public void Exit()
        {
            entity.animator.SetBool("IsPrepare", false);
            entity.CancelInvoke(nameof(entity.Charge));
        }
    }
}