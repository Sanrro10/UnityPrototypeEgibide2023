using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace StatePattern.PlayerStates
{
    public class DashState: IState
    {
        private PlayerController player;
        
        public DashState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Dash State");
            
            player.StartCoroutine(player.DashDuration());
            player.InvokeRepeating(nameof(player.Dash), 0, 0.01f);
        }

        public void Update()
        {
            if (!player.isDashing)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.IdleState);
            }
            
        }

        public void Exit()
        {
            player.CancelInvoke(nameof(player.Dash));
        }
    }
}
