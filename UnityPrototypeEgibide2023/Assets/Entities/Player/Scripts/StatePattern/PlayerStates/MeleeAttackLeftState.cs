using Entities.Player.Scripts;
using UnityEngine;
namespace StatePattern.PlayerStates
{
    public class MeleeAttackLeftState : IState
    {
        private PlayerController player;
        
        public MeleeAttackLeftState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Left Attack State");
            
            
            player.isInMiddleOfAttack = true;
            player.animator.SetBool("IsALeft", true);
            player.Invoke(nameof(player.EndAttack), 0.8f);
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (!player.isInMiddleOfAttack)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.IdleState);
                return;
            }
        }
        
        public void Exit()
        {
            player.CancelInvoke(nameof(player.EndAttack));
            player.canAttack = true;
            player.isInMiddleOfAttack = false;

            player.animator.SetBool("IsALeft", false);
            
            Debug.Log("Exit Left Attack State");
        }
    }
}
