using System;
using UnityEngine;

namespace Entities
{
    public class EntityControler : MonoBehaviour
    {
        protected HealthComponent Health;
        protected bool Invulnerable;
        protected float InvulnerableTime = 0.5f;
        protected Rigidbody2D Rb;
    
        void Awake()
        {
            Health = gameObject.AddComponent(typeof(HealthComponent)) as HealthComponent;
        }

        private void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        public virtual void OnDeath()
        {
  
        }

        public virtual void OnReceiveDamage(int damage)
        {
            if (Invulnerable) return;
           Health.RemoveHealth(damage); 
           Invoke(nameof(DamageCooldown), InvulnerableTime);
        }
        
        public void DamageCooldown()
        {
            Invulnerable = false;
            Rb.WakeUp();
        }
    }
}
