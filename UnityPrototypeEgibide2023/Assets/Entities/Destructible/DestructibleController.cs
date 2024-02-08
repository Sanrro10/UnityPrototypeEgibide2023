using UnityEngine;

namespace Entities.Destructible
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Animator))]
    public class DestructibleController : EntityControler
    {
        [SerializeField] private AttackComponent.AttackType vulnerabilityType = AttackComponent.AttackType.Normal;

        [SerializeField] private int numberOfHitsToDestroy = 1;

        private Animator _animator;
        private static readonly int FailedHit = Animator.StringToHash("FailedHit");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Remove = Animator.StringToHash("Remove");

        // Start is called before the first frame update
        void Start()
        {
            Health.Set(numberOfHitsToDestroy);
            _animator = GetComponent<Animator>();
        }
    
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        {
            if (attack.attackType != vulnerabilityType)
            {
                _animator.SetTrigger(FailedHit);
                return;
            }
            _animator.SetTrigger(Hit);
            Health.RemoveHealth(1);
        }
    
        public override void OnDeath()
        {
            _animator.SetTrigger(Remove);
        }
    }
}
