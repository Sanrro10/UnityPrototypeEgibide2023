using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;


namespace Entities.Enemies.Goat.Scripts.StatePattern.States
{
    public class GoatIdleState : IState
    {
        // Start is called before the first frame update
        private GoatBehaviour entity;
        
        public GoatIdleState(GoatBehaviour entity)
        {
            this.entity = entity;
        }

        public void Enter()
        {
            entity.animator.SetBool("IsIdle", true);
            entity.InvokeRepeating(nameof(entity.LookForEnemy), 0, 0.01f);
        
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
        
        }
        
        public void Exit()
        {
            entity.CancelInvoke(nameof(entity.LookForEnemy));
            entity.animator.SetBool("IsIdle", false);
        }
    }
}

