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
            goatBehaviour = GetComponentInParent <GoatBehaviour>();
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Wall") || other.CompareTag("Floor"))
            {
                goatBehaviour.BounceAgainstWall();
                return;
            }

        }
    }
}
