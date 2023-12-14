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
            player.canAttack = false;
            player.isPerformingMeleeAttack = true;
            Debug.Log("Entering Left Attack State");
            player.animator.SetTrigger("MeleeLeftAttack");
            player.Invoke(nameof(player.AttackDuration), player.meleeAttackDuration);
            player.Invoke(nameof(player.AttackCooldown), player.meleeAttackCooldown );
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            if (player.canAttack && player.isPerformingMeleeAttack) player.GroundAttack();
            
        }
        
        public void Exit()
        {
            
            player.canAttack = true;
            player.isPerformingMeleeAttack = false;

        }
    }
}
