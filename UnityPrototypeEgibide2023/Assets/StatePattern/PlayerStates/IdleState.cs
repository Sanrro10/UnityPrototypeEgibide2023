
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class IdleState: IState
    {
        private PlayerController player;
        
        public IdleState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            // Debug.Log("Entering Idle State");
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            if (!player.IsGrounded())
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.AirState);
            }
            
            
            
            // if we press the jump button, transition to the jump state
            
            // if we press the attack button, transition to the attack state
            
            // if we press the dash button, transition to the dash state
            
            
            
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Idle State");
        }
    }
}