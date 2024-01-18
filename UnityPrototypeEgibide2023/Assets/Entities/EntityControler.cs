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
        public bool FacingRight = true;
    
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

        public virtual void OnReceiveDamage(int damage, float knockback, Vector2 angle, bool facingRight = true)
        { 
            if (Invulnerable) return;
           Health.RemoveHealth(damage); 
           Debug.Log("Damage:" + damage);
           Invulnerable = true;
           Invoke(nameof(DamageCooldown), InvulnerableTime);
           Push(knockback, angle, facingRight);
        }
        
        public virtual void Invulneravility()
        {
            Invulnerable = true;
        }
        
        public virtual void EndInvulneravility()
        {
            Invulnerable = false;
        }

        public virtual void Push(float knockback, Vector2 angle, bool facingRight)
        {
            
        }
        
        public void DamageCooldown()
        {
            Invulnerable = false;
            //Rb.WakeUp();
        }
    }
}
