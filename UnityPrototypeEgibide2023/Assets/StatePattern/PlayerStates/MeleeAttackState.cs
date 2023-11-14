using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class MeleeAttackState : IState
    {
        private PlayerController player;
        
        public MeleeAttackState(PlayerController player)
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
            if (player.isMoving)
            {
                player.Move();
            }
            if (player.isPerformingMeleeAttack && player.canAttack)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.MeleeAttackState);

            }
            
            if (player.isJumping)
            {
                player.pmStateMachine.TransitionTo(player.pmStateMachine.JumpState);
                return;
            }
            
        }
        
        public void Exit()
        {
 
        }
    }
}
