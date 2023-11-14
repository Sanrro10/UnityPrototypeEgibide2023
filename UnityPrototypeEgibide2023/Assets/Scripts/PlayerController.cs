using System;
using System.Collections;
using StatePattern;
using StatePattern.PlayerStates;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController: EntityControler
{
        // Components
        public PlayerMovementStateMachine PmStateMachine { get; private set; }
        private InputActions _controls;
        private Rigidbody2D _rigidbody2D;
        [SerializeField] private GameObject feet;
        [SerializeField] private BoxCollider2D feetBoxCollider; 
        [SerializeField] private PlayerData playerData;
        public Animator animator;
        private SpriteRenderer _spriteRenderer;
        
        
        // internal state controls
        public bool isHoldingHorizontal = false;
        public bool isHoldingVertical = false;
        public bool isJumping = false;
        public bool isDashing = false;
        public bool onDJump = false;
        public bool facingRight = true;
        public bool isCollidingLeft = false;
        public bool isCollidingRight = false;
        public bool isStunned = false;
        
        // internal state variables
        public float horizontalSpeed;
        public float dashSpeed;
        public float jumpForce;

        [SerializeField] private AnimationCurve dashCurve;
        public bool onDashCooldown = false;
        [SerializeField] private GameObject feet;
        public Animator animator;
        private SpriteRenderer _spriteRenderer;
        public bool onDashCooldown = false;
        public float maxAirHorizontalSpeed;
        public float maxFallSpeed;
        public float timeStunned;
        
        private AnimationCurve _dashCurve;
        private int _numberOfGrounds;
        
        [SerializeField] private float floatDuration;

        
        void Start()
        {
                //Initialize components
                animator = GetComponent<Animator>();
                _spriteRenderer = GetComponent<SpriteRenderer>();
                _controls = new InputActions();
                _rigidbody2D = GetComponent<Rigidbody2D>();
                
                //Enable the actions
                _controls.Enable();
                
                // Initialize the state machine
                PmStateMachine = new PlayerMovementStateMachine(this);
                PmStateMachine.Initialize(PmStateMachine.IdleState);
                
                //Inputs
                _controls.GeneralActionMap.HorizontalMovement.started += ctx => isHoldingHorizontal = true;
                _controls.GeneralActionMap.HorizontalMovement.canceled += ctx => isHoldingHorizontal = false;
                _controls.GeneralActionMap.VerticalMovement.started += ctx =>  isHoldingVertical = true;
                _controls.GeneralActionMap.VerticalMovement.canceled += ctx =>  isHoldingVertical = false;
                //Jump
                _controls.GeneralActionMap.Jump.started += ctx => isJumping = true;
                _controls.GeneralActionMap.Jump.canceled += ctx => isJumping = false;

                //Dash -> Add Force in the direction the player is facing
                _controls.GeneralActionMap.Dash.performed += ctx => isDashing = true;
                
                
                // Initialize data
                horizontalSpeed = playerData.movementSpeed;
                maxAirHorizontalSpeed = playerData.maxAirHorizontalSpeed;
                _numberOfGrounds = 0;
                _rigidbody2D.gravityScale = playerData.gravity;
                _dashCurve = playerData.dashCurve;
                maxFallSpeed = playerData.maxFallSpeed;
                
        }

        private void FixedUpdate()
        {
                //Debug.Log(IsGrounded());
                PmStateMachine.StateUpdate();
                
                // Clamp gravity
                Vector2 clampVel = _rigidbody2D.velocity;
                clampVel.y = Mathf.Clamp(clampVel.y, -maxFallSpeed, 9999);
                _rigidbody2D.velocity = clampVel;
                
        }

        public void ClampVelocity(float x, float y)
        {
                Vector2 clampVel = _rigidbody2D.velocity;
                clampVel.y = Mathf.Clamp(clampVel.y, -y, y);
                clampVel.x = Mathf.Clamp(clampVel.x, -x, x);
                _rigidbody2D.velocity = clampVel;
        }



        public void Jump()
        {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, jumpForce);
        }

        public void Move()
        {
                FlipSprite();
                
                if((facingRight && isCollidingRight) || (!facingRight && isCollidingLeft))
                {
                        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y); 
                        return;
                }

                _rigidbody2D.velocity =
                        new Vector2((facingRight ? horizontalSpeed : horizontalSpeed * -1), _rigidbody2D.velocity.y); 
        }

        public void AirMove()
        {
                FlipSprite();
                float airAcceleration = 1f;
                if((facingRight && isCollidingRight) || (!facingRight && isCollidingLeft))
                {
                        _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y); 
                        return;
                }

                if (facingRight)
                {
                        if (_rigidbody2D.velocity.x > maxAirHorizontalSpeed)
                        {
                                return;
                        }
                        _rigidbody2D.velocity =
                                new Vector2(_rigidbody2D.velocity.x + airAcceleration, _rigidbody2D.velocity.y);
                        
                }

                if (!facingRight)
                {
                        if (_rigidbody2D.velocity.x < -maxAirHorizontalSpeed)
                        {
                                return;
                        }
                        
                        _rigidbody2D.velocity =
                                new Vector2(_rigidbody2D.velocity.x - airAcceleration, _rigidbody2D.velocity.y);
                }
                
                
                
        }
        
        public bool IsGrounded()
        {
                return 0 < _numberOfGrounds;
        }
        
        public void EndDash()
        {
                
                _rigidbody2D.velocity =
                        new Vector2((facingRight ? dashSpeed : (dashSpeed -10) * -1), 0); 
                Debug.Log("IsDashing");
                
        }
        
        public void EndStun()
        {
                isStunned = false;
        }

        public bool CanDash()
        {
                if (!onDashCooldown && isDashing) 
                {
                        return true;
                }
                else
                {
                        return false;
                }
        }

        public void FlipSprite()
        {
                
                float direction = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();

                if (direction == -1) facingRight = false;
                else if (direction == 1) facingRight = true;
                _spriteRenderer.flipX = !facingRight;

        }

        public void AirDashStart()
        {
                
        }

        public void StunEntity(float time)
        {
                timeStunned = time;
                PmStateMachine.TransitionTo(PmStateMachine.StunnedState);
        }
        public void AirDash()
        {
                float xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                float yDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();
                
                float xForce = playerData.airdashForce * xDirection;
                float yForce = playerData.airdashForce * yDirection;


                if (yForce == 0)
                {
                        yForce = playerData.airdashForce / 2;
                }
                _rigidbody2D.velocity = new Vector2(xForce, yForce * 2);


        }
        
        
        
        // -------------- COROUTINES -----------------
        public IEnumerator Dash()
        {
                Physics2D.IgnoreLayerCollision(6,7, true);
                float dashTime = 0;
                float dashSpeedCurve = 0;
                Debug.Log("Dash Duration: " + _dashCurve.keys[_dashCurve.length - 1].time);
                while (dashTime < _dashCurve.keys[_dashCurve.length - 1].time)
                {
                        dashSpeedCurve = _dashCurve.Evaluate(dashTime) * dashSpeed; 
                        _rigidbody2D.velocity = new Vector2((facingRight ? dashSpeedCurve : dashSpeedCurve * -1), 0); 
                        yield return new WaitForFixedUpdate(); 
                        dashTime += Time.deltaTime;
                }
                Physics2D.IgnoreLayerCollision(6,7, false);
                isDashing = false;
                
                
        }

        public IEnumerator AirDashDuration()
        {
                yield return new WaitForSeconds(playerData.airdashDuration);
                PmStateMachine.TransitionTo(PmStateMachine.AirState);
        }
        public IEnumerator FloatDuration()
        {
                
                yield return new WaitForSeconds(playerData.floatDuration);
                PmStateMachine.TransitionTo(PmStateMachine.AirDashState);
        }

        public IEnumerator GroundedCooldown()
        {
                feetBoxCollider.enabled = false;
                yield return new WaitForSeconds(0.2f);
                feetBoxCollider.enabled = true;
        }
        public IEnumerator GroundedDashCooldown()
        {
                onDashCooldown = true;
                yield return new WaitForSeconds(playerData.dashCooldown);
                onDashCooldown = false;
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
                }
        }

        
        // Getters and setters
        public void SetNumberOfGrounds(int numberOfGrounds)
        {
                this._numberOfGrounds = numberOfGrounds;
        }

        public int GetNumberOfGrounds()
        {
                return this._numberOfGrounds;
        }
        
        public void SetXVelocity(float i)
        {
                _rigidbody2D.velocity = new Vector2(i, _rigidbody2D.velocity.y);
        }

        public void SetYVelocity(float i)
        {
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, i);
        }

        public void RestartGravity()
        {
                _rigidbody2D.gravityScale = playerData.gravity;
        }
        
        public void SetGravity(float i)
        {
                _rigidbody2D.gravityScale = i;
        }
        
        // --------------- EVENTS ----------------------

        public IEnumerator MaxJumpDuration()
        {
                yield return new WaitForSeconds(playerData.jumpDuration);
                isJumping = false;

        }

        public void setXVelocity(float i)
        {
                _rigidbody2D.velocity = new Vector2(i, _rigidbody2D.velocity.y);
        }

        public PlayerData GetPlayerData()
        { 
                return playerData;
        }

        public void ReceiveDamage(int damage) 
        {
                Debug.Log(_health.Get());
                _health.RemoveHealth(damage);
                Debug.Log(_health.Get());
        } 
}