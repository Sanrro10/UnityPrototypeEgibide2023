using UnityEngine;

namespace Entities
{
    public class AttackZarza : AttackComponent
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            attackData.ForEach((attack) =>
            {
                if (attack.layer == other.gameObject.layer)
                {
                    var otherEntity = other.GetComponent<EntityControler>();
                    if (otherEntity == null) return;
                    other.GetComponent<EntityControler>().OnReceiveDamage(attack, toTheRight);
                }
            });
            
        }
        
        private void OnTriggerStay2D(Collider2D other)
        {
            attackData.ForEach((attack) =>
            {
                if (attack.layer == other.gameObject.layer)
                {
                    var otherEntity = other.GetComponent<EntityControler>();
                    if (otherEntity == null) return;
                    other.GetComponent<EntityControler>().OnReceiveDamage(attack, toTheRight);
                }
            });
            
        }
        
    }
}