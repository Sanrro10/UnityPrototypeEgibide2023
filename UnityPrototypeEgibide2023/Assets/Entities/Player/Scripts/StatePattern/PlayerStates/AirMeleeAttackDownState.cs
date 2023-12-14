using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AirMeleeAttackDownState : IState
    {
        private PlayerController player;
        
        public AirMeleeAttackDownState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.canAttack = false;
            player.isPerformingMeleeAttack = true;
            Debug.Log("Entering air down Attack State");
            player.animator.SetTrigger("AirMeleeDownAttack");
            player.Invoke(nameof(player.AttackDuration), player.meleeAttackDuration );
            player.Invoke(nameof(player.AttackCooldown), player.meleeAttackCooldown );
            
        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {            
        }
        
        public void Exit()
        {
            player.canAttack = true;
            player.isPerformingMeleeAttack = false;

        }
    }
}
