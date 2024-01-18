using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    public class GoatFrontTrigger : MonoBehaviour
    {
    
        [SerializeField] private GoatBehaviour goatBehaviour;
        // Start is called before the first frame update

        private void Awake()
        {
            goatBehaviour = GetComponentInParent < GoatBehaviour>();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (goatBehaviour.canCollideWithPlayer == false) return;
            if (other.CompareTag("Wall"))
            {
                goatBehaviour.canCollideWithPlayer = false;
                goatBehaviour.BounceAgainstWall();
                return;
            }

            if (other.gameObject.layer == 6)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                goatBehaviour.BounceAgainstPlayer();
                player.StunEntity(goatBehaviour.data.stunTime);
                
            }
        }
    }
}
