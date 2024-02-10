using UnityEngine;

namespace Enviroment.Platforms.Scripts
{
    public class PlayerOneWayPlatform : MonoBehaviour
    {

        private Collider2D _collider;
        private GameObject _target;
        private void Start()
        {
            _target = GameObject.FindWithTag("Player");
            _collider = GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), true);
        }
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerHelper") && other.name == "Feet")
            {
                if (_target.GetComponent<Rigidbody2D>().velocity.y <= 0)
                {
                    Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), false);
                    return;
                }
                Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), true);
            }


        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("PlayerHelper") && other.name == "Feet") Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), true);
        }
    
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("PlayerHelper") && other.name == "Feet")
            {
                if (_target.GetComponent<Rigidbody2D>().velocity.y <= 0)
                {
                    Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), false);
                    return;
                }
                Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), true);
            }
        }
    
    }
}
