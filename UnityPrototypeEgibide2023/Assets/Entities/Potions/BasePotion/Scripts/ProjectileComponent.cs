using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    public class ProjectileComponent : MonoBehaviour
    {
        private AttackComponent[] _attackComponents;
        
        public float delay = 0f;
        public float damageActiveTime = 0.5f;
        public float destroyTime = 1f;
        public int speed = 0;
        void Awake()
        {
            _attackComponents = GetComponentsInChildren<AttackComponent>(true);
            Invoke(nameof(ActivateHitbox), delay);

            Despawn();
        }
        
        protected virtual void ActivateHitbox()
        {
            foreach(AttackComponent ac in _attackComponents)
            {
                ac.ActivateHitbox(damageActiveTime);
            }
        }
        
        protected virtual void Despawn()
        {
            Invoke(nameof(this.Destroy), delay + destroyTime);
        }
        
        private void Destroy()
        {
            Destroy(gameObject);
        }

    }
}
