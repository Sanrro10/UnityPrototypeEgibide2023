
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AirState: IState
    {
        private PlayerController player;
        
        public AirState(PlayerController player)
        {
            this.player = player;
        }
        
        public void Enter()
        {
            Debug.Log("Entering Air State");
            Debug.Log(player.IsGrounded());
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            
            if (player.IsGrounded())
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.IdleState);
                return;
            }
            
            if (player.isDashing)
            {
                player.pmStateMachine.TransitionTo((player.pmStateMachine.DashState));
                return;
            }
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Air State");
            
        }
    }
}