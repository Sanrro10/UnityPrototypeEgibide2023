using UnityEngine;

namespace Entities
{
    public class EntityControler : MonoBehaviour
    {
        protected HealthComponent _health;
    
        void Awake()
        {
            _health = gameObject.AddComponent(typeof(HealthComponent)) as HealthComponent;
        }
    
        public virtual void OnDeath()
        {
  
        }
    }
}
