using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirDashState : AirState
    {
        
        public AirDashState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.AirDash();
            Player.StartCoroutine(Player.AirDashDuration());
            Player.StartCoroutine(Player.DashCooldown());
            // Debug.Log("Entering Air Dash State");
            // Initialize Dash
        }

        
        public override void Exit()
        {
            base.Exit();
            Player.StopCoroutine(Player.AirDashDuration());
            

        }
    }
}