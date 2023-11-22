using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class MeleeAttackDownState : IState
    {
        private PlayerController player;
        
        public MeleeAttackDownState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            //Debug.Log("Entering Attack State");
            player.animator.SetTrigger("MeleeAttack");
            player.canAttack = false;
            player.Invoke(nameof(player.AttackCooldown), player.meleeAttackCooldown );
            player.Invoke(nameof(player.AttackDuration), player.meleeAttackDuration );
            
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            /*if (player.isJumping)
            {
                player.PmStateMachine.TransitionTo(player.PmStateMachine.JumpState);
                return;
            }*/
            
        }
        
        public void Exit()
        {
 
        }
    }
}
