using System.Collections.Generic;
using UnityEngine;

namespace Entities
{
    public class AttackComponent : MonoBehaviour
    {
        [System.Serializable]
        public struct AttackData
        {
            public AttackData(int damage, float knockback, Vector2 angle, int layer)
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

        public void Start()
        {
            attackData ??= new List<AttackData>();
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
            var found = attackData.Find((attack) => attack.layer == other.gameObject.layer);
            if (found.Equals(default(AttackData))) return;
            var entity = other.GetComponent<EntityControler>();
            if (entity == null) return;
            other.GetComponent<EntityControler>().OnReceiveDamage(found.damage, found.knockback, found.angle);
        }
    
    }
}
