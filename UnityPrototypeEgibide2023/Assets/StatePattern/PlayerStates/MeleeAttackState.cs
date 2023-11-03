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
            player.animator.SetTrigger("MeleeAttack");
            Debug.Log("Entering Attack State");
            player.canAttack = false;
            player.Invoke(nameof(player.AttackCooldown), player.melleeAttackCooldown );

        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
            
            
        }
        
        public void Exit()
        {
            // Debug.Log("Exiting Idle State");
        }
    }
}
