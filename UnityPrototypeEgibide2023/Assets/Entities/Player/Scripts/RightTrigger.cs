using UnityEngine;

namespace Entities.Player.Scripts
{
    public class RightTrigger : MonoBehaviour
    {
        private BoxCollider2D _collider2D;
        private int _numberOfGrounds = 0;
        [SerializeField] private PlayerController _playerController;
        // Start is called before the first frame update
        void Start()
        {
            _collider2D = GetComponent<BoxCollider2D>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Floor") || other.CompareTag("Wall") || other.CompareTag("Enemy"))
            {
                _numberOfGrounds++;
                _playerController.isCollidingRight = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Floor") || other.CompareTag("Wall") || other.CompareTag("Enemy"))
            {
                _numberOfGrounds--;
                if (_numberOfGrounds == 0)  _playerController.isCollidingRight = false;
            }    
        }
    }
}
