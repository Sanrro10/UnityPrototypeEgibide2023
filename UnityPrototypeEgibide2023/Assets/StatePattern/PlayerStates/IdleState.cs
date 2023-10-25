﻿
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
            Debug.Log("Entering Idle State");
            player.onDJump = false;
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            // If we're no longer grounded, transition to the air state
            if (player.isMoving)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.WalkState);
                return;
            }

            if (player.isDashing)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.DashState);
                return;
            }

            if (player.isJumping)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.JumpState);
                return;
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