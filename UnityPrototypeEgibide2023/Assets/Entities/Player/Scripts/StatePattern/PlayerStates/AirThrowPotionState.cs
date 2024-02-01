using StatePattern;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirThrowPotionState : AirState
    {

        public AirThrowPotionState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.animator.SetBool("IsAirThrowing", true);
            Player.Invoke(nameof(Player.ThrowPotionAir), 0.1f);
            Player.Invoke(nameof(Player.EndThrowPotionAir), Player.GetPlayerData().potionThrowDuration);
            Player.InvokeRepeating(nameof(Player.AirMove), 0, 0.01f);
        }

        public override void Update()
        {
            base.Update();

            if (Player.CanAirDash())
            {
                Player.PmStateMachine.TransitionTo((Player.PmStateMachine.AirDashStartState));
                return;
            }

            if (Player.isPerformingMeleeAttack)
            {
                Player.AirAttack();
            }
        }

        public override void Exit()
        {
            Player.CancelInvoke(nameof(Player.AirMove));
            Player.CancelInvoke(nameof(Player.ThrowPotionAir));
            Player.CancelInvoke(nameof(Player.EndThrowPotionAir));
            Player.animator.SetBool("IsAirThrowing", false);
        }
    }
}