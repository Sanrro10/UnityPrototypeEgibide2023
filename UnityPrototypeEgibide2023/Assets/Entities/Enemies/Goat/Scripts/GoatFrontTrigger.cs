using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
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
                return;
            }
        
        

            if (other.CompareTag("Player"))
            {
            
                other.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2((goatBehaviour.facingRight ? 1 : -1)* 1000f , 1000f));
                goatBehaviour.BounceAgainstPlayer();
                other.GetComponentInParent<PlayerController>().StunEntity(goatBehaviour.stunTime);
            }
        
        }
    }
}
