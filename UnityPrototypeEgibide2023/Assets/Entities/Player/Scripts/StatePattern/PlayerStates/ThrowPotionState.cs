using StatePattern;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class ThrowPotionState : GroundState
    {
        
        public ThrowPotionState(PlayerController player) : base (player)
        {
        }
        
        public override void Enter()
        {
            base.Enter();
            Player.animator.SetBool("IsThrowing", true);
            Player.Invoke(nameof(Player.ThrowPotion), 0.1f);
            Player.Invoke(nameof(Player.EndThrowPotion), Player.GetPlayerData().potionThrowDuration);
        }

        public override void Update()
        {
            base.Update();
            if (Player.isPerformingJump)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.JumpState);
                return;
            }
            
            if (Player.CanDash())
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.GroundDashState);
                return;
            }
        }

        public override void Exit()
        {
            base.Exit();
            Player.CancelInvoke(nameof(Player.ThrowPotion));
            Player.CancelInvoke(nameof(Player.EndThrowPotion));
            Player.animator.SetBool("IsThrowing", false);
        }
    }
}