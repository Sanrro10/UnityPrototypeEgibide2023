using StatePattern;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class ThrowPotionState : IState
    {
        private PlayerController player;
        
        public ThrowPotionState(PlayerController player)
        {
            this.player = player;
        }
        
        public void Enter()
        {
            
        }

        public void Update()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}