using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    public class PotionBehavior : EntityControler
    {
        [SerializeField] private Vector2 spawnForce;

        [SerializeField] private BasePotionData data;

        private bool _hasBeenHitted = false;
        void Start()
        {
            Health.Set(data.health);
            InvulnerableTime = 0.2f;
        }
        
        public override void OnReceiveDamage(int damage, float knockback, Vector2 angle, bool facingRight = true)
        {
            if (!_hasBeenHitted) Rb.velocity = new Vector2();
            // _hasBeenHitted = true;
            base.OnReceiveDamage(0, knockback, angle, facingRight);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Explode();
            
        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 7)
            {
                Explode();
                return;
            }
            Bounce(1);
        }

    
        private void Bounce(int damage)
        {
            Health.RemoveHealth(damage);
        }
        
    

        private void Explode()
        {   
            Instantiate(data.explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
