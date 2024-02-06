using System;
using Entities.Enemies.Galtzagorri.Scripts.StatePattern.States;
using StatePattern;

namespace Entities.Enemies.Galtzagorri.Scripts.StatePattern
{
    public class GaltzStateMachine : IStateMachine
    {
        public IState CurrentState { get; private set; }
        public event Action<IState> OnStateChanged;
        
        public GaltzAttackState GaltzAttackState;
        public GaltzDeathState GaltzDeathState;
        public GaltzHiddenState GaltzHiddenState;
        public GaltzHidingState GaltzHidingState;
        public GaltzRunningState GaltzRunningState;

        public GaltzStateMachine(NewGaltzScript entity)
        {
            this.GaltzAttackState = new GaltzAttackState(entity);
            this.GaltzDeathState = new GaltzDeathState(entity);
            this.GaltzHiddenState = new GaltzHiddenState(entity);
            this.GaltzHidingState = new GaltzHidingState(entity);
            this.GaltzRunningState = new GaltzRunningState(entity);
        }
        
        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
            
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
