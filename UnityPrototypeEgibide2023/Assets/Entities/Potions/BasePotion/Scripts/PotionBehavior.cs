using Entities.Player.Scripts;
using UnityEngine;
using UnityEngine.WSA;

namespace Entities.Potions.BasePotion.Scripts
{
    public class PotionBehavior : EntityControler
    {
        

        [SerializeField] private BasePotionData data;
        
        void Start()
        {
            Health.Set(data.health);
            InvulnerableTime = 0.2f;
        }

        public override void OnReceiveDamage(int damage, float knockback, Vector2 angle, bool facingRight = true)
        {
            Health.RemoveHealth(1); 
            Invulnerable = true;
            Invoke(nameof(DamageCooldown), InvulnerableTime);
            
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Explode();
        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            // if collision is against floor 
            if (collision.gameObject.layer == 7)
            {
                Explode();
                return;
            }
            Bounce(1);
        }

        private void ApplyForce()
        {
            
        
        
        }

    
        private void Bounce(int damage)
        {
            Health.RemoveHealth(damage);
            
        }
    

        private void Explode()
        {   
            Instantiate(data.explosion, transform.position, Quaternion.identity);
            // Add particle effects
            Destroy(gameObject);
        }

    }
}
