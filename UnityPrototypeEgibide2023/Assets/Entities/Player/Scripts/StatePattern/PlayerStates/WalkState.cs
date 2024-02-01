using StatePattern;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class WalkState : GroundState
    {
        
        public WalkState(PlayerController player) : base(player)
        {
        }

        public override void Enter()
        {
            base.Enter();
            Player.animator.SetBool("IsWalk", true);
            Player.InvokeRepeating(nameof(Player.Move), 0, 0.01f);
        }

        // per-frame logic, include condition to transition to a new state
        public override void Update()
        {
            base.Update();
            if (Player.isPerformingJump)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.JumpState);
                return;
            }

            if (!Player.isHoldingHorizontal)
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.IdleState);
                return;
            }
            
            if (Player.CanDash())
            {
                Player.PmStateMachine.TransitionTo((Player.PmStateMachine.GroundDashState));
                return;
            }

            if (Player.isPerformingMeleeAttack)
            {
                Player.GroundAttack();
                return;
            }

            if (Player.CanThrowPotion())
            {
                Player.PmStateMachine.TransitionTo(Player.PmStateMachine.ThrowPotionState);
                return;
            }

        }
        
        public override void Exit()
        {
            base.Exit();
            
            Player.CancelInvoke(nameof(Player.Move));
            Player.animator.SetBool("IsWalk", false);
        }
    }
}