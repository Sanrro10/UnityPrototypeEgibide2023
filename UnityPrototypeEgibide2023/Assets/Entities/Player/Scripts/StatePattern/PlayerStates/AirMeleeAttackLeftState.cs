using Entities.Player.Scripts;
using UnityEngine;
namespace StatePattern.PlayerStates
{
    public class AirMeleeAttackLeftState : IState
    {
        private PlayerController player;
        
        public AirMeleeAttackLeftState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.canAttack = false;
            player.isPerformingMeleeAttack = true;
            Debug.Log("Entering Air Left Attack State");
            player.animator.SetTrigger("AirMeleeLeftAttack");
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
