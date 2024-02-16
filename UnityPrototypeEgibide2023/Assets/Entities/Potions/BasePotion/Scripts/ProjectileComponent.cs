using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    public class ProjectileComponent : MonoBehaviour
    {
        private AttackComponent[] _attackComponents;
        private AudioSource _audioSource;
        public float delay = 0f;
        public float damageActiveTime = 0.5f;
        public float destroyTime = 1f;
        public int speed = 0;
        public Audios _audios;
        void Awake()
        {
            _attackComponents = GetComponentsInChildren<AttackComponent>(true);
            Invoke(nameof(ActivateHitbox), delay);
            _audioSource ??= GetComponent<AudioSource>();
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
            if ( _audioSource != null ) {
                _audioSource.clip ??= _audios.audios[0];
                _audioSource?.Play();
            }
            Destroy(gameObject);
            
        }

    }
}
