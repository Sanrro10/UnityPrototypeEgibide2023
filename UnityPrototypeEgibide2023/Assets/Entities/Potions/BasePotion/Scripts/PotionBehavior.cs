using System;
using Entities.Destructible;
using Entities.Player.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Potions.BasePotion.Scripts
{
    public class PotionBehavior : EntityControler
    {

        [SerializeField] private BasePotionData data;
        private bool _canBounce = true;
        private bool _hasBeenHitted = false;
        public static event Action<GameObject> OnPotionDestroy;
        void Start()
        {
            Health.Set(data.health);
            InvulnerableTime = 0.1f;
            GetComponent<Rigidbody2D>().angularVelocity = Random.Range(-200, 200);
        }
        
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool facingRight = true)
        {
            if (!Invulnerable && !_hasBeenHitted)  Rb.velocity = new Vector2();
            attack.damage = 1;
            if (Health.Get() <= 1) attack.damage = 0;
            base.OnReceiveDamage(attack, facingRight);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            Explode();
        }


        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 7 || collision.gameObject.GetComponent<DestructibleController>() != null)
            {
                Explode();
                return;
            }
            Bounce(1);
        }

    
        protected virtual void Bounce(int damage)
        {
            if (!_canBounce) return;
            Health.RemoveHealth(damage);
            Invoke(nameof(SetBounce), 0.05f);
            
        }

        private void SetBounce()
        {
            _canBounce = true;
        }
    

        protected virtual void Explode()
        {   
            Instantiate(data.explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
            OnPotionDestroy?.Invoke(gameObject);
        }

    }
}
