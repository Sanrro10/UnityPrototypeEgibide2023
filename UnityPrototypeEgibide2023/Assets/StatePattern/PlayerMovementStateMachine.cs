using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using StatePattern.PlayerStates;
using UnityEngine;

namespace StatePattern
{
    [Serializable]
    public class PlayerMovementStateMachine : IStateMachine
    {
        public IState CurrentState { get; private set; }
        
        // Event to notify other objects that state has changed
        public event Action<IState> OnStateChanged;

        // We add all possible states here
        public IdleState IdleState;
        public WalkState WalkState;
        public AirState AirState;
        public DashState DashState;
        public JumpState JumpState;
        public DJumpState DJumpState;
        
        
        // Constructor
        public PlayerMovementStateMachine(PlayerController player)
        {
            this.IdleState = new IdleState(player);
            this.WalkState = new WalkState(player);
            this.AirState = new AirState(player);
            this.DashState = new DashState(player);
            this.JumpState = new JumpState(player);
            this.DJumpState = new DJumpState(player);
        }
        
        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
            
            // Notify other objects that state has changed
            OnStateChanged?.Invoke(CurrentState);
        }

        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            nextState.Enter();
            
            OnStateChanged?.Invoke(CurrentState);
        }

        public void StateUpdate()
        {
            if (CurrentState != null)
            {
                CurrentState.Update();
            }
        }
    }
    
}

