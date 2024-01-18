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

        public void Start()
        {
            attackData ??= new List<AttackData>();
            entity ??= GetComponentInParent<EntityControler>();
        }

        public void AddAttackData(AttackData ad)
        {
            this.attackData.Add(ad);
        }

        public void ClearAttackData()
        {
            this.attackData.Clear();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            attackData.ForEach((attack) =>
            {
                if (attack.layer == other.gameObject.layer)
                {
                    var otherEntity = other.GetComponent<EntityControler>();
                    if (otherEntity == null) return;
            
                    other.GetComponent<EntityControler>().OnReceiveDamage(attack.damage, attack.knockback, attack.angle, entity.FacingRight);
                }
            });
            
        }
    
    }
}
