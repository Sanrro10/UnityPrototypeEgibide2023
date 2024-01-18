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
        
            if (other.CompareTag("Wall"))
            {
                goatBehaviour.BounceAgainstWall();
                return;
            }
            
            if (other.gameObject.layer == 6) goatBehaviour.BounceAgainstPlayer();
        }
    }
}
