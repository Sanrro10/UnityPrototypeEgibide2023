using System.Collections;
using StatePattern;
using StatePattern.PlayerStates;
using UnityEngine;

public class PlayerController: MonoBehaviour
{
        public PlayerMovementStateMachine pmStateMachine { get; private set; }
        
        
        private InputActions _controls;
        public bool facingRight = true;
        public bool isMoving = false;
        public bool isJumping = false;
        public bool isDashing = false;
        public bool onDJump = false;
        public float horizontalSpeed;
        public float maxVerticalSpeed;
        public bool isCollidingLeft = false;
        public bool isCollidingRight = false;
        public float dashSpeed;
        public float dashDuration;
        public float airdashForce;
        public float jumpForce;
        [SerializeField] private GameObject feet;
        public Animator animator;
        private SpriteRenderer _spriteRenderer;
        private int _numberOfGrounds;
        private Rigidbody2D _rigidbody2D;
        private ConstantForce2D _force2D;
        [SerializeField] private float floatDuration;

        [SerializeField] private BoxCollider2D feetBoxCollider;

        [SerializeField] private PlayerData playerData;
        void Start()
        {
                //_force2D = GetComponent<ConstantForce2D>();
                animator = GetComponent<Animator>();
                _spriteRenderer = GetComponent<SpriteRenderer>();
                _controls = new InputActions();
                _numberOfGrounds = 0;
                _rigidbody2D = GetComponent<Rigidbody2D>();
                horizontalSpeed = playerData.movementSpeed;
                //Enable the actions
                _controls.Enable();
                pmStateMachine = new PlayerMovementStateMachine(this);
                pmStateMachine.Initialize(pmStateMachine.IdleState);
                //Inputs
                _controls.GeneralActionMap.Movement.started += ctx => isMoving = true;
                _controls.GeneralActionMap.Movement.canceled += ctx => isMoving = false;
                
                //Jump
                _controls.GeneralActionMap.Jump.started += ctx => isJumping = true;
                
                _controls.GeneralActionMap.Jump.canceled += ctx => isJumping = false;

                //Dash -> Add Force in the direction the player is facing
                _controls.GeneralActionMap.Dash.performed += ctx => isDashing = true;
                
        }

        private void FixedUpdate()
        {
                //Debug.Log(IsGrounded());
                pmStateMachine.StateUpdate();
                Vector2 clampVel = _rigidbody2D.velocity;
                clampVel.y = Mathf.Clamp(clampVel.y, -maxVerticalSpeed, 9999);

                _rigidbody2D.velocity = clampVel;
        }



        public void Jump()
        {
                _rigidbody2D.velocity = Vector2.up * jumpForce;
                
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
        
        public bool IsGrounded()
        {
                return 0 < _numberOfGrounds;
        }


        
        public void Dash()
        {
                
                _rigidbody2D.velocity =
                        new Vector2((facingRight ? dashSpeed : dashSpeed * -1), 0); 
                Debug.Log("IsDashing");
                
        }

        public void FlipSprite()
        { 
                Vector2 direccion = _controls.GeneralActionMap.Movement.ReadValue<Vector2>();
                facingRight = direccion.x == 1 ? true : false;
                _spriteRenderer.flipX = !facingRight;

        }
        public void AirDash()
        {
                Vector2 direction = _controls.GeneralActionMap.Movement.ReadValue<Vector2>();
                float hDirection = direction.x;
                float vDirection = direction.y;

                _rigidbody2D.velocity = new Vector2(hDirection * airdashForce, vDirection * airdashForce);
        }
        
        
        
        // -------------- COROUTINES -----------------
        public IEnumerator DashDuration()
        {
                yield return new WaitForSeconds(dashDuration);
                isDashing = false;
        }

        public IEnumerator AirDashDuration()
        {
                yield return new WaitForSeconds(floatDuration);
                pmStateMachine.TransitionTo(pmStateMachine.AirState);
        }
        public IEnumerator FloatDuration()
        {
                _rigidbody2D.gravityScale = 0;
                yield return new WaitForSeconds(0.5f);
                pmStateMachine.TransitionTo(pmStateMachine.AirDashState);
        }

        public IEnumerator GroundedCooldown()
        {
                feetBoxCollider.enabled = false;
                yield return new WaitForSeconds(0.2f);
                feetBoxCollider.enabled = true;
        }

        
        
        public void setNumberOfGrounds(int numberOfGrounds)
        {
                this._numberOfGrounds = numberOfGrounds;
        }

        public int getNumberOfGrounds()
        {
                return this._numberOfGrounds;
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
}