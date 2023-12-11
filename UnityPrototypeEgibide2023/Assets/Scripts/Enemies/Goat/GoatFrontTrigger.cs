using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoatFrontTrigger : MonoBehaviour
{
    private BoxCollider2D _collider2D;
    
    [SerializeField] private GoatBehaviour goatBehaviour;
    // Start is called before the first frame update

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!goatBehaviour.canCollide)
        {
            return;
        }
        
        if (other.CompareTag("Wall"))
        {
            goatBehaviour.BounceAgainstWall();
            goatBehaviour.canCollide = false;
            return;
        }
        
        

        if (other.CompareTag("Player"))
        {
            
            other.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2((goatBehaviour.facingRight ? 1 : -1)* 1000f , 1000f));
            goatBehaviour.BounceAgainstPlayer();
            other.GetComponentInParent<PlayerController>().StunEntity(goatBehaviour.stunTime);
            goatBehaviour.canCollide = false;
        }
        
    }
}
