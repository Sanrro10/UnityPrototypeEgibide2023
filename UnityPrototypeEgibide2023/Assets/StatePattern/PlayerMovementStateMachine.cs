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
        public GroundDashState GroundDashState;
        public JumpState JumpState;
        public DJumpState DJumpState;
        public AirDashState AirDashState;
        public AirDashStartState AirDashStartState;
        
        // Constructor
        public PlayerMovementStateMachine(PlayerController player)
        {
            this.IdleState = new IdleState(player);
            this.WalkState = new WalkState(player);
            this.AirState = new AirState(player);
            this.GroundDashState = new GroundDashState(player);
            this.JumpState = new JumpState(player);
            this.DJumpState = new DJumpState(player);
            this.AirDashState = new AirDashState(player);
            this.AirDashStartState = new AirDashStartState(player);
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

