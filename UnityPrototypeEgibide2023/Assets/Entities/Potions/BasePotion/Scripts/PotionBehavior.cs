using System;
using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    public class PotionBehavior : EntityControler
    {

        [SerializeField] private BasePotionData data;
        
        public static event Action<GameObject> OnPotionDestroy;
        private bool _hasBeenHitted = false;
        void Start()
        {
            Health.Set(data.health);
            InvulnerableTime = 0.1f;
        }
        
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool facingRight = true)
        {
            if (!Invulnerable && !_hasBeenHitted)  Rb.velocity = new Vector2();
            attack.damage = 0;
            base.OnReceiveDamage(attack, facingRight);
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
            OnPotionDestroy?.Invoke(gameObject);
        }

    }
}
