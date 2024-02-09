using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;


namespace Entities.Enemies.Goat.Scripts.StatePattern.States
{
    public class GoatDeathState : IState
    {
        // Start is called before the first frame update
        private GoatBehaviour entity;
        
        public GoatDeathState(GoatBehaviour entity)
        {
            this.entity = entity;
        }

        public void Enter()
        {
            entity.animator.SetBool("IsDeath", true);
            AnimationClip currentAnim = entity.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            entity.StopAllCoroutines();
            entity.CancelInvoke();
            entity.Invoke(nameof(entity.DestroyEntity), currentAnim.length + 2f);

        }

        // per-frame logic, include condition to transition to a new state
        public void Update()
        {
        
        }
        
        public void Exit()
        {
            entity.animator.SetBool("IsDeath", false);
        }
    }
}