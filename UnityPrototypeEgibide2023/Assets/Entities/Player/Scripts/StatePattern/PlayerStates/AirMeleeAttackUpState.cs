using Entities.Player.Scripts;
using UnityEngine;
namespace StatePattern.PlayerStates
{
    public class AirMeleeAttackUpState : IState
    {
        private PlayerController player;
        
        public AirMeleeAttackUpState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.canAttack = false;
            player.isPerformingMeleeAttack = true;
            Debug.Log("Entering Air Up Attack State");
            player.animator.SetTrigger("AirMeleeUpAttack");
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
            Debug.Log("Exits Air Up Attack State");
        }
    }
}
