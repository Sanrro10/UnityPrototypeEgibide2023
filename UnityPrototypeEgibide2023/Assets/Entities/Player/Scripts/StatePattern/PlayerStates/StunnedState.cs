using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class StunnedState : IState
    {
        private PlayerController player;
        public StunnedState(PlayerController player)
        {
            this.player = player;
        }
        
        public void Enter()
        {
            //  Debug.Log("Entering Stunned State");
            player.Invoke(nameof(player.EndStun), 0.25f);
            player.isStunned = true;
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (!player.isStunned)
            {
               if(player.IsGrounded()) 
                   player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
               else
                   player.PmStateMachine.TransitionTo(player.PmStateMachine.AirborneState);   
            }
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Stunned State");
        }  
    }
}