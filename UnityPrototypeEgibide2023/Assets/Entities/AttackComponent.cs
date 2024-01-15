using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class AttackComponent : MonoBehaviour
{
    public int damage;
    public int damageLayer;
private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == damageLayer)
        {
            other.GetComponent<EntityControler>().OnReceiveDamage(damage);
            other.GetComponent<EntityControler>().Invulneravility();
            
            // TODO: Maybe add knockback
            return;
        }
    }
    
}
