using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirborneState: AirState
    {
        public AirborneState(PlayerController player) : base(player)
        {
        }
        
        public override void Enter()
        {
            // Debug.Log("Entering Air State");
            base.Enter();
            Player.animator.SetBool("IsAir", true);
            Player.InvokeRepeating(nameof(Player.AirMove), 0, 0.01f);
        }
        
        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();
            
            if (Player.isPerformingMeleeAttack)
            {
                Player.AirAttack();
                return;
            }
            
            if (Player.CanAirDash())
            {
                Player.PmStateMachine.TransitionTo((Player.PmStateMachine.AirDashStartState));
                return;
            }
            
            if (Player.CanThrowPotion())
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.AirThrowPotionState);
                return;
            }

        }
        
        public override void Exit()
        {
            base.Exit();
            Player.CancelInvoke(nameof(Player.AirMove));
            Player.animator.SetBool("IsAir", false);
        }
    }
}