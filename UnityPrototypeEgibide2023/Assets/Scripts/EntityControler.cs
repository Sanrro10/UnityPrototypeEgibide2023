using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
