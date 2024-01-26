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
            player.animator.SetBool("IsThrowing", true);
            player.Invoke(nameof(player.ThrowPotion), 0.1f);
            player.Invoke(nameof(player.EndThrowPotion), player.GetPlayerData().potionThrowDuration);
        }

        public void Update()
        {
            if (player.isPerformingJump)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.JumpState);
                return;
            }

            if (!player.IsGrounded())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.AirState);
                
                return;
            }
            
            if (player.CanDash())
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.GroundDashState);
                return;
            }
        }

        public void Exit()
        {
            player.CancelInvoke(nameof(player.ThrowPotion));
            player.CancelInvoke(nameof(player.EndThrowPotion));
            player.animator.SetBool("IsThrowing", false);
        }
    }
}