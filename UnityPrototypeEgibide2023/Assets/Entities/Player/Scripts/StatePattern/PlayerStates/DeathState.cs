using StatePattern;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class DeathState : IState
    {
        private PlayerController Player;
        public DeathState(PlayerController player)
        {
            this.Player = player;
        }
        
        public void Enter()
        {
            //  Debug.Log("Entering Stunned State");
            Player.animator.SetBool("IsDeath", true);
            Player.isStunned = true;
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
        }
        
        public void Exit()
        {
            Player.animator.SetBool("IsDeath", false);
            // Debug.Log("Exiting Stunned State");
        }  
    }
}