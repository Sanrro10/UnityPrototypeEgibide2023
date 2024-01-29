using System;
using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class AttackComponent : MonoBehaviour
    {
        [System.Serializable]
        public struct AttackData
        {
            public AttackData(int damage, float knockback, Vector2 angle, int layer, AttackType? attackType)
            {
                this.damage = damage;
                this.knockback = knockback;
                this.angle = angle;
                this.layer = layer;
                this.attackType = attackType ?? AttackType.Normal;
            }
            public int damage;
            public float knockback;
            public Vector2 angle;
            public int layer;
            public AttackType attackType;
        }
        
        [System.Serializable]
        public enum AttackType
        {
            Normal,
            Explosion,
            Projectile,
            Electric
        }
    
        public List<AttackData> attackData;
        public EntityControler entity;
        public AttackType attackType;
        public bool toTheRight = true;

        public static event Action<EntityControler, EntityControler> OnHit;
        
        public void Start()
        {
            attackData ??= new List<AttackData>();
            entity ??= GetComponentInParent<EntityControler>();
        }

        public virtual void AddAttackData(AttackData ad)
        {
            this.attackData.Add(ad);
        }

        public virtual void ClearAttackData()
        {
            this.attackData.Clear();
        }

        public virtual void DeactivateHitbox()
        {
            GetComponent<Collider2D>().enabled = false;
        }
        
        public virtual void ActivateHitbox()
        {
            GetComponent<Collider2D>().enabled = true;
        }

        public virtual void ActivateHitbox(float time)
        {
            GetComponent<Collider2D>().enabled = true;
            Invoke(nameof(DeactivateHitbox), time);
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            attackData.ForEach((attack) =>
            {
                if (attack.layer == other.gameObject.layer)
                {
                    var otherEntity = other.GetComponent<EntityControler>();
                    if (otherEntity == null) return;
                    if (entity != null) OnHit?.Invoke(entity, otherEntity);
                    other.GetComponent<EntityControler>().OnReceiveDamage(attack, (entity == null ? toTheRight : entity.FacingRight));
                }
            });
            
        }
    
    }
}
