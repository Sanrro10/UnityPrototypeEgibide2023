using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates 
{
    public class SceneChangeState : IState
    {

        private PlayerController player;

        public SceneChangeState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public void Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}
