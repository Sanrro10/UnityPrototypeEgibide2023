using System.Collections;
using Entities.Enemies.Goat.Scripts.StatePattern;
using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    public class GoatBehaviour : EntityControler
    {
        public bool facingRight;
        public bool canCollide = true;
        public bool collidedWithPlayer = false;
        [SerializeField] public GoatData data;
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
            animator = GetComponent<Animator>();
            stateMachine = new GoatStateMachine(this);
            stateMachine.Initialize(stateMachine.GoatIdleState);
            Health.Set(data.health);
        }

        public void Charge()
        {
            stateMachine.TransitionTo(stateMachine.GoatChargeState);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            stateMachine.TransitionTo(stateMachine.GoatDeathState);
        }


        private void Death()
        {
            CancelInvoke(nameof(Move));
            Destroy(this);
        }
    
        // Move the goat using the rigidbody2D
        public void Move()
        {
            _rb.velocity = new Vector2(data.movementSpeed * (facingRight ? 1 : -1), _rb.velocity.y);
        }
    
        public void Jump() 
        {
            _rb.AddForce(new Vector2(_rb.velocity.x, data.jumpForce));
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
                yield return 0.1f;
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
            if(wasPlayer) stateMachine.TransitionTo(stateMachine.GoatIdleState);
            else stateMachine.TransitionTo(stateMachine.GoatSpinState);
        }
    

        public void LookForEnemy()
        {
            Debug.DrawRay(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left) * data.visionDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, (facingRight ? Vector2.right : Vector2.left), data.visionDistance , playerLayer);
        
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
            _rb.velocity = new Vector2(_rb.velocity.x * -0.5f, data.jumpForce);
        }
        
        public void BounceAgainstPlayer()
        {
            collidedWithPlayer = true;
            stateMachine.TransitionTo(stateMachine.GoatStunnedState);
        }
    }
}
