using StatePattern;
using UnityEngine;

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
            Debug.Log("Entering Death State");
            Player.animator.SetBool("IsDeath", true);
            Player.Invulnerability();
        }
        
        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
        }
        
        public void Exit()
        {
            Player.animator.SetBool("IsDeath", false);
            Player.EndInvulnerability();
            Debug.Log("Exiting Death State");
        }  
    }
}