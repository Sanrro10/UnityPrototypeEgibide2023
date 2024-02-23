using StatePattern;
using UnityEngine;

namespace Entities.Player.Scripts.StatePattern.PlayerStates
{
    public class AirState : IState
    {
        protected PlayerController Player;

        protected AirState(PlayerController player)
        {
            this.Player = player;
        }
        
        public virtual void Enter()
        {
            Player.GetRigidbody().drag = 1;
            Player.GetRigidbody().inertia = 2;
        }

        public virtual void Update()
        {
            if (Player.IsGrounded() && Player.GetRigidbody().velocity.y < 0.3f)
            {
                Player.particleEvents.PlayJumpParticles();
                if (Player.isHoldingHorizontal)
                    Player.PmStateMachine.TransitionTo(Player.PmStateMachine.WalkState);
                else
                    Player.PmStateMachine.TransitionTo(Player.PmStateMachine.IdleState);
                return;
            }
        }

        public virtual void Exit()
        {
            
        }
    }
}