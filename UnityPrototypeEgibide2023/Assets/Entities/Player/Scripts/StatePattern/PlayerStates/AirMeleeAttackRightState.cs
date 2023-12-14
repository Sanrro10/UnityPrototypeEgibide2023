using Entities.Player.Scripts;
using UnityEngine;

namespace StatePattern.PlayerStates
{
    public class AirMeleeAttackRightState : IState
    {
        private PlayerController player;
        
        public AirMeleeAttackRightState(PlayerController player)
        {
            this.player = player;
        }

        public void Enter()
        {
            player.canAttack = false;
            player.isPerformingMeleeAttack = true;
            Debug.Log("Entering Air Right Attack State");
            player.animator.SetTrigger("AirMeleeRightAttack");
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
