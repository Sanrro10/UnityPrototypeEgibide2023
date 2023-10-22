
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
            // Debug.Log("Entering Air State");
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Air State");
        }
    }
}