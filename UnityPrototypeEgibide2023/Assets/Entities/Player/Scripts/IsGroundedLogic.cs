using UnityEngine;

namespace Entities.Player.Scripts
{
    public class IsGroundedLogic : MonoBehaviour
    {

        [SerializeField] private GameObject player;

        private PlayerController pc;
        // Start is called before the first frame update
        void Start()
        {
            pc = player.GetComponent<PlayerController>();
        }
    
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals("Floor"))
            {
                pc.SetNumberOfGrounds(pc.GetNumberOfGrounds() + 1);
            }
        
        }

        private void OnTriggerExit2D(Collider2D collision)
        {

            if (collision.gameObject.tag.Equals("Floor"))
            {
                pc.SetNumberOfGrounds(pc.GetNumberOfGrounds() - 1);
            }
        }
    }
}
