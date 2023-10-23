namespace StatePattern
{
    public class PlayerCombatStateMachine : IStateMachine
    {
        public IState CurrentState { get; private set; }
        public event System.Action<IState> OnStateChanged;
        
        public void Initialize(IState startingState)
        {
            CurrentState = startingState;
            CurrentState.Enter();
        }

        public void TransitionTo(IState nextState)
        {
            CurrentState.Exit();
            CurrentState = nextState;
            CurrentState.Enter();
            OnStateChanged?.Invoke(CurrentState);
        }

        public void Update()
        {
            CurrentState.Update();
        }
    }
}