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
        public bool canCollideWithPlayer = false;
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
            Vector2 startRotation = transform.eulerAngles;
            float endRotation = startRotation.x + 180.0f;
            float t = 0.0f;
            while ( t  < 0.5f )
            {
                t += Time.deltaTime;
                float yRotation = Mathf.Lerp(startRotation.x, endRotation, t / 0.5f);
                transform.eulerAngles = new Vector2(startRotation.x, yRotation);
                yield return 0.1f;
            }

            stateMachine.TransitionTo(stateMachine.GoatPrepareState);
        }

        public IEnumerator HasStopped(bool wasPlayer)
        {
            yield return 0.1f;
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
            _rb.velocity = new Vector2(_rb.velocity.x * -1f, data.jumpForce);
        }
        
        public void BounceAgainstPlayer()
        {
            collidedWithPlayer = true;
            stateMachine.TransitionTo(stateMachine.GoatStunnedState);
        }
    }
}
