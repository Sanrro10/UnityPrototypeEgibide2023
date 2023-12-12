using System;

namespace StatePattern
{
    public interface IStateMachine
    {
        public IState CurrentState { get; }
        public event Action<IState> OnStateChanged;
        
        public void Initialize(IState startingState);
        public void TransitionTo(IState nextState);
        public void StateUpdate();
        
    }
}