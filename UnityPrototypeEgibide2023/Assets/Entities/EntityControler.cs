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
        public bool FacingRight;

    
        void Awake()
        {
            Health = gameObject.AddComponent(typeof(HealthComponent)) as HealthComponent;
            Rb ??= GetComponent<Rigidbody2D>();
        }
        

        public virtual void OnDeath()
        {
            
        }

        public virtual void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        { 
            if (Invulnerable) return;
           Health.RemoveHealth(attack.damage); 
           Debug.Log("Damage:" + attack.damage);
           Invulnerable = true;
           Invoke(nameof(DamageCooldown), InvulnerableTime);
           Push(attack.knockback, attack.angle, toTheRight);
        }
        
        public virtual void Invulneravility()
        {
            Invulnerable = true;
        }
        
        public virtual void EndInvulneravility()
        {
            Invulnerable = false;
        }

        public virtual void Push(float knockback, Vector2 angle, bool toTheRight = true)
        {
            Debug.Log(knockback + " " + angle + " " + toTheRight);
            if (toTheRight)
            {
                Rb.AddForce(new Vector2(angle.x * knockback, angle.y * knockback));
            }
            else
            {
                Rb.AddForce(new Vector2(-(angle.x * knockback), angle.y * knockback));
            }
        }

        
        public void DamageCooldown()
        {
            Invulnerable = false;
            //Rb.WakeUp();
        }
        
        public Rigidbody2D GetRigidbody()
        {
            return Rb;
        }
    }
}
