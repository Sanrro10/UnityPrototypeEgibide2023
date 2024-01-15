using System.Collections;
using Entities.Enemies.Goat.Scripts.StatePattern;
using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    public class GoatBehaviour : EntityControler
    {
        public float speed;
        public float force;
        public float jumpForce;
        public bool facingRight;
        public float stunTime;
        public bool canCollide = true;
        public float waitTime = 0.5f;
        public bool collidedWithPlayer = false;
        [SerializeField] private LayerMask playerLayer;
        // reference to player

        private Rigidbody2D _rb;

        public GoatStateMachine stateMachine;
        
        [SerializeField] public Animator animator;

        [SerializeField] private GameObject eyes;
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            stunTime = 0.5f;
            force = 500f;
            animator = GetComponent<Animator>();
            stateMachine = new GoatStateMachine(this);
            stateMachine.Initialize(stateMachine.GoatIdleState);
        }

        public void Charge()
        {
            stateMachine.TransitionTo(stateMachine.GoatChargeState);
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


        public IEnumerator TurnAround()
        {
            int newEulerY = -1;
            
            while(newEulerY != 180 && newEulerY != 0)
            {
                transform.eulerAngles = new Vector3(transform.transform.eulerAngles.x,
                    transform.rotation.eulerAngles.y - (facingRight ? 10 : -10), transform.rotation.eulerAngles.z);
                newEulerY = (int)transform.rotation.eulerAngles.y;
                yield return 0.05f;
            }
        
            facingRight = !facingRight;

            stateMachine.TransitionTo(stateMachine.GoatPrepareState);
        }

        public IEnumerator HasStopped(bool wasPlayer)
        {
            while (_rb.velocity != new Vector2(0, 0))
            {
                yield return 0.1f;
            }
            Debug.Log("It was player: " + wasPlayer + "");
            if(wasPlayer) stateMachine.TransitionTo(stateMachine.GoatIdleState);
            else stateMachine.TransitionTo(stateMachine.GoatSpinState);
        }
    

        public void LookForEnemy()
        {
            Debug.DrawRay(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left) * 3f, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left), 3f, playerLayer);
        
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    stateMachine.TransitionTo(stateMachine.GoatPrepareState);
                    
                }
            }
        }

        public void BounceAgainstWall()
        {
            collidedWithPlayer = false;
            stateMachine.TransitionTo(stateMachine.GoatStunnedState);
        }

        public void Bounce()
        {
            _rb.velocity = new Vector2(_rb.velocity.x * -1, jumpForce);
        }
        
        public void BounceAgainstPlayer()
        {
            collidedWithPlayer = true;
            stateMachine.TransitionTo(stateMachine.GoatStunnedState);
        }
    }
}
