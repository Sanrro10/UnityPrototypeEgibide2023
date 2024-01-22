using Entities.Player.Scripts;
using UnityEngine;
using UnityEngine.WSA;

namespace Entities.Potions.BasePotion.Scripts
{
    public class PotionBehavior : EntityControler
    {
        [SerializeField] private Vector2 spawnForce;

        [SerializeField] private BasePotionData data;
        
        private Vector2 _lastVelocity;
        void Start()
        {
            Health.Set(data.health);
            InvulnerableTime = 0.2f;
        }
        
        public override void OnReceiveDamage(int damage, float knockback, Vector2 angle, bool facingRight = true)
        {
            Rb.velocity = new Vector2();
            base.OnReceiveDamage(damage, knockback, angle, facingRight);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            
        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            // if collision is against enemy 
            if (collision.gameObject.layer == 7)
            {
                Explode();
                return;
            }
            Bounce(1);
        }

    
        private void Bounce(int damage)
        {
            _lastVelocity = Rb.velocity;
            Rb.velocity = new Vector2();
            float verticalVelocity = 0;
            if (_lastVelocity.y <= 0)
            {
                verticalVelocity = -8;
            }
            else
            {
                verticalVelocity = 8;
            }
            
            if (_lastVelocity.x < 0)
            {
                Rb.velocity = new Vector2(8, verticalVelocity);
            }
            else
            {
                Rb.velocity = new Vector2(-8, verticalVelocity);
            }
            Health.RemoveHealth(damage);
        }
    

        private void Explode()
        {   
            Instantiate(data.explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
