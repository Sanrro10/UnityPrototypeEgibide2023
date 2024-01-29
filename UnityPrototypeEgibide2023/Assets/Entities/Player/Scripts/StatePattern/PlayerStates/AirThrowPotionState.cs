using StatePattern;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirThrowPotionState : IState
    {
        private PlayerController player;

        public AirThrowPotionState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.animator.SetBool("IsAirThrowing", true);
            player.Invoke(nameof(player.ThrowPotionAir), 0.1f);
            player.Invoke(nameof(player.EndThrowPotionAir), player.GetPlayerData().potionThrowDuration);
            player.InvokeRepeating(nameof(player.AirMove), 0, 0.01f);
        }

        public void Update()
        {
            if (player.IsGrounded())
            {
                if (player.isHoldingHorizontal)
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.WalkState);
                else
                    player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }

            if (player.CanAirDash())
            {
                player.PmStateMachine.TransitionTo((player.PmStateMachine.AirDashStartState));
                return;
            }

            if (player.isPerformingMeleeAttack)
            {
                player.AirAttack();
            }
        }

        public void Exit()
        {
            player.CancelInvoke(nameof(player.AirMove));
            player.CancelInvoke(nameof(player.ThrowPotionAir));
            player.CancelInvoke(nameof(player.EndThrowPotionAir));
            player.animator.SetBool("IsAirThrowing", false);
        }
    }
}