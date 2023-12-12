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
            player.canAttack = false;
            player.isPerformingMeleeAttack = false;
            Debug.Log("Entering Right Attack State");
            player.animator.SetTrigger("MeleeAttack");
            player.Invoke(nameof(player.AttackDuration), player.meleeAttackDuration );
            player.Invoke(nameof(player.AttackCooldown), player.meleeAttackCooldown );
            player.Invoke(nameof(player.SpawnAttackHitbox), player.meleeAttackStart);
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
