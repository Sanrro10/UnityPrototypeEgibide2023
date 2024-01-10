using System;
using Entities.Enemies.Goat.Scripts.StatePattern.States;
using StatePattern;

namespace Entities.Enemies.Goat.Scripts.StatePattern
{
    public class GoatStateMachine : IStateMachine
    {
        public IState CurrentState { get; private set; }
        
        // Event to notify other objects that state has changed
        public event Action<IState> OnStateChanged;

        // We add all possible states here
        public GoatIdleState GoatIdleState;
        public GoatChargeState GoatChargeState;
        public GoatSpinState GoatSpinState;
        public GoatStunnedState GoatStunnedState;
        public GoatDeathState GoatDeathState;
        public GoatPrepareState GoatPrepareState;
        
        // Constructor
        public GoatStateMachine(GoatBehaviour entity)
        {
            this.GoatIdleState = new GoatIdleState(entity);
            this.GoatChargeState = new GoatChargeState(entity);
            this.GoatSpinState = new GoatSpinState(entity);
            this.GoatStunnedState = new GoatStunnedState(entity);
            this.GoatDeathState = new GoatDeathState(entity);
            this.GoatPrepareState = new GoatPrepareState(entity);
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
