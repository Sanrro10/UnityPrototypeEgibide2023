using System;
using System.Collections;
using Entities.Enemies.Goat.Scripts.StatePattern;
using JetBrains.Annotations;
using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    public class GoatBehaviour : EntityControler
    {
        [SerializeField] private AudioGoatScript audioGoat;
        [SerializeField] public GoatData data;
        [SerializeField] private LayerMask playerLayer;

        [SerializeField] public GameObject attackHitbox;

        [NotNull] public AttackComponent AttackComponent;

        // reference to player
        public bool canCollideWithPlayer = false;
        public bool collidedWithPlayer = false;

        public GoatStateMachine stateMachine;
        
        [SerializeField] public Animator animator;

        [SerializeField] private GameObject eyes;
        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            stateMachine = new GoatStateMachine(this);
            stateMachine.Initialize(stateMachine.GoatIdleState);
            Health.Set(data.health);
            AttackComponent = attackHitbox.GetComponent<AttackComponent>();
            AttackComponent.AddAttackData(new AttackComponent.AttackData(data.damage, data.knockback, data.angle, 6, AttackComponent.AttackType.Normal));
            AttackComponent.DeactivateHitbox();
            AttackComponent.OnHit += OnHit;
        }
        
        private void OnHit(EntityControler attacker, EntityControler victim)
        {
            if (attacker == this) 
                BounceAgainstPlayer();
        }

        private void OnDestroy()
        {
            AttackComponent.OnHit -= OnHit;
        }

        public void Charge()
        {
            stateMachine.TransitionTo(stateMachine.GoatChargeState);
        }

        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool facingRight = true)
        {
            base.OnReceiveDamage(attack, FacingRight);
        }

        public override void OnDeath()
        {
            base.OnDeath();
            stateMachine.TransitionTo(stateMachine.GoatDeathState);
        }
        
        public void DestroyEntity()
        {
            Destroy(gameObject);
        }


        private void Death()
        {
            CancelInvoke(nameof(Move));
            Destroy(this);
        }
    
        // Move the goat using the rigidbody2D
        public void Move()
        {
            Rb.velocity = new Vector2(data.movementSpeed * (FacingRight ? 1 : -1), Rb.velocity.y);
        }
        
        public void Jump() 
        {
            Rb.AddForce(new Vector2(Rb.velocity.x, data.jumpForce));
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
            while (Rb.velocity != new Vector2(0, 0))
            {
                yield return 0.1f;
            }
            if(wasPlayer) stateMachine.TransitionTo(stateMachine.GoatIdleState);
            else stateMachine.TransitionTo(stateMachine.GoatSpinState);
        }
    

        public void LookForEnemy()
        {
            Debug.DrawRay(eyes.transform.position, (FacingRight ? Vector2.right : Vector2.left) * data.visionDistance, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(eyes.transform.position, (FacingRight ? Vector2.right : Vector2.left), data.visionDistance , playerLayer);
        
            if (hit.collider != null)
            {
                if (hit.collider.CompareTag("PlayerHelper"))
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
            Rb.velocity = new Vector2(Rb.velocity.x * -1f, data.jumpForce);
        }
        
        public void BounceAgainstPlayer()
        {
            collidedWithPlayer = true;
            stateMachine.TransitionTo(stateMachine.GoatStunnedState);
        }
    }
}
