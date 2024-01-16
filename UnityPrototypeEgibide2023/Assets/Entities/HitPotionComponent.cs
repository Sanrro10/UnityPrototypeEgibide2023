using UnityEngine;

namespace Entities
{
    public class PushComponent : MonoBehaviour
    {
        public float[] force;

        private void OnTriggerEnter2D(Collider2D other)
        {
            /* if (other.gameObject.layer == damageLayer)
            {
                other.GetComponent<EntityControler>().OnReceiveDamage(damage);
                other.GetComponent<EntityControler>().Invulneravility();
                other.GetComponent<EntityControler>().Knockback(force);
            */
        }
    }
}