using System.Collections;
using Entities.Player.Scripts;
using UnityEngine;

namespace Entities.Enemies.Goat
{
    public class GoatBehaviour : MonoBehaviour
    {
        public float speed;
        public float force;
        public float jumpForce;
        public bool facingRight;
        public float stunTime;
        public bool canCollide = true;

        [SerializeField] private LayerMask playerLayer;
        // reference to player

        private Rigidbody2D _rb;

        [SerializeField] private GameObject eyes;
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            InvokeRepeating(nameof(LookForEnemy), 0, 0.01f);
        }

        // Update is called once per frame
        public void ActivateEnemy()
        {
            canCollide = true;
            InvokeRepeating(nameof(Move), 0, 0.01f);
            CancelInvoke(nameof(LookForEnemy));
        }
    

        private void Death()
        {
            CancelInvoke(nameof(Move));
            Destroy(this);
        }
    
        // Move the goat using the rigidbody2D
        public void Move()
        {
            _rb.velocity = new Vector2(speed * (facingRight ? 1 : -1), _rb.velocity.y);
        }
    
        public void Jump() 
        {
            _rb.AddForce(new Vector2(_rb.velocity.x, jumpForce));
        }
    
        // Get the direction the goat is facing

    

        private IEnumerator TurnAround()
        {
        
            while(Mathf.Abs(this.transform.rotation.eulerAngles.y - (facingRight ? 180 : 0)) > 0.3f)
            {
                this.transform.Rotate(new Vector3(0, 1, 0), 180f * Time.deltaTime);
                yield return Time.deltaTime;
            }
        
            facingRight = !facingRight;

            ActivateEnemy();
        }
    
        private IEnumerator HasStopped(bool wasPlayer)
        {
            while (_rb.velocity != new Vector2(0, 0))
            {
                yield return 0.1f;
            }

            canCollide = true;
            if(!wasPlayer) StartCoroutine(TurnAround());
            else ActivateEnemy();
        }
    

        private void LookForEnemy()
        {
            Debug.DrawRay(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left) * 3f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left), 3f, playerLayer);
        
            if (hit.collider != null)
            {

                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Player"))
                {
                    ActivateEnemy();
                }
            }
        }

        public void BounceAgainstWall()
        {
            CancelInvoke(nameof(Move));
            StartCoroutine(HasStopped(false));
        
            // Add force to the goat like a jump
            _rb.velocity = new Vector2(_rb.velocity.x * -1, jumpForce);
        
        }

        public void BounceAgainstPlayer(GameObject player)
        {
            CancelInvoke(nameof(Move));
            StartCoroutine(HasStopped(true));
            player.GetComponentInParent<Rigidbody2D>().AddForce(new Vector2((facingRight ? 1 : -1) * force, 1000f));
     
            player.GetComponentInParent<PlayerController>().StunEntity(stunTime);

            // Add force to the goat like a jump
            _rb.velocity = new Vector2(_rb.velocity.x * -1, jumpForce);
        }
    }
}
