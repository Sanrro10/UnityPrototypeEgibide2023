using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class MeleeAttackRightState : IState
    {
        private PlayerController player;
        
        public MeleeAttackRightState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            Debug.Log("Entering Right Attack State");
            
            
            player.isInMiddleOfAttack = true;
            player.animator.SetBool("IsARight", true);
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

            player.animator.SetBool("IsARight", false);
            
            Debug.Log("Exit Right Attack State");
        }
    }
}
