using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;


namespace Entities.Enemies.Goat.Scripts.StatePattern.States
{
    public class GoatStunnedState : IState
    {
        // Start is called before the first frame update
        private GoatBehaviour entity;
        
        public GoatStunnedState(GoatBehaviour entity)
        {
            this.entity = entity;
        }

        public void Enter()
        {

        
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
        
        }
        
        public void Exit()
        {
        }
    }
}