using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class AttackComponent : MonoBehaviour
    {
        [System.Serializable]
        public struct AttackData
        {
            public AttackData(int damage, float knockback, Vector2 angle, LayerMask layer)
            {
                this.damage = damage;
                this.knockback = knockback;
                this.angle = angle;
                this.layer = layer;
            }
            public int damage;
            public float knockback;
            public Vector2 angle;
            public int layer;
        }
    
        public List<AttackData> attackData;
        public EntityControler entity;
        public bool toTheRight = true;
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
                    other.GetComponent<EntityControler>().OnReceiveDamage(attack.damage, attack.knockback, attack.angle, (entity == null ? toTheRight : entity.FacingRight));
                }
            });
            
        }
    
    }
}
