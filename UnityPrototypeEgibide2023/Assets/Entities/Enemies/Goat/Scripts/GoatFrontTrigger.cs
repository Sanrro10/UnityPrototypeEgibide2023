using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Goat
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
                goatBehaviour.canCollide = false;
                return;
            }
        
        

            if (other.CompareTag("Player"))
            {
            
               goatBehaviour.BounceAgainstPlayer(other.gameObject);
               goatBehaviour.canCollide = false;
            }
        
        }
    }
}
