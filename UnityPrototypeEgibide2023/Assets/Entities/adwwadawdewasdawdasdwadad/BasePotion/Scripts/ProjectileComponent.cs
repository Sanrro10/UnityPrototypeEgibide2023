using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    public class ProjectileComponent : MonoBehaviour
    {
        private AttackComponent[] _attackComponents;

        public float damageActiveTime = 0.5f;
        public float destroyTime = 1f;
        public int speed = 0;
        void Awake()
        {
            _attackComponents = GetComponentsInChildren<AttackComponent>(true);
            foreach(AttackComponent ac in _attackComponents)
            {
                ac.ActivateHitbox(damageActiveTime);
            }

            Despawn();
        }
        
        protected virtual void Despawn()
        {
            Invoke(nameof(this.Destroy), destroyTime);
        }
        
        private void Destroy()
        {
            Destroy(gameObject);
        }

    }
}
