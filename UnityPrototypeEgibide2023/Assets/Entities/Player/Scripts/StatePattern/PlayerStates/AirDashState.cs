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
            Player.StartCoroutine(Player.AirDashDuration());
            Player.StartCoroutine(Player.AirDashCooldown());
            // Debug.Log("Entering Air Dash State");
            // Initialize Dash
        }

        
        public override void Exit()
        {
            base.Exit();
            Player.StopCoroutine(Player.AirDashDuration());
            Player.RestartGravity();

        }
    }
}