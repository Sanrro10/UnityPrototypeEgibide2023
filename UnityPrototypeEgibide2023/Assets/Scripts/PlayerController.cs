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
        public bool onDashCooldown = false;
        public bool onDJump = false;
        public float horizontalSpeed;
        public float horizontalAcceleration;
        public float verticalSpeed;
        public float verticalAcceleration;
        public float maxHorizontalSpeed;
        public float maxVerticalSpeed;
        public float gravity;
        public float friction;
        public float airdashForce;
        private float _jumpForce;
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

        private void Update()
        {
                //Debug.Log(IsGrounded());
                pmStateMachine.StateUpdate();
                
        }

        private void CalculateVertical()
        {
                
        }
        
        private void CalculateHorizontal()
        {
                horizontalSpeed += horizontalAcceleration;
                if (horizontalSpeed > maxHorizontalSpeed)
                        horizontalSpeed -= friction;
                
                transform.position += new Vector3(horizontalSpeed, 0, 0);
        }

        public void Jump()
        {
                _rigidbody2D.velocity = Vector2.up * playerData.jumpPower;
                
        }

        public void Move()
        {
                Vector2 direccion = _controls.GeneralActionMap.Movement.ReadValue<Vector2>();
                facingRight = direccion.x == 1 ? true : false;
                _spriteRenderer.flipX = !facingRight;
                //_force2D.relativeForce = new Vector2(facingRight ? horizontalSpeed : horizontalSpeed * -1, 0);
        }
        
        public bool IsGrounded()
        {
                return 0 < _numberOfGrounds;
        }

        public void SetCurrentGravity(float gravity)
        {
                this.gravity = gravity;
        }

        public void ResetGravity()
        {
                this.gravity = playerData.gravity;
        }
        
        public void Dash()
        {
                float dashValue = (playerData.movementSpeed * 100) * playerData.dashSpeed;
                dashValue *= _controls.GeneralActionMap.Movement.ReadValue<Vector2>().x;
                _rigidbody2D.velocity = Vector2.right * dashValue;
                _rigidbody2D.gravityScale = 0;
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
                yield return new WaitForSeconds(playerData.dashDuration);
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.gravityScale = 2;
                pmStateMachine.TransitionTo(pmStateMachine.AirState);
        }

        public IEnumerator AirDashDuration()
        {
                yield return new WaitForSeconds(floatDuration);
                _rigidbody2D.velocity = Vector2.zero;
                pmStateMachine.TransitionTo(pmStateMachine.AirState);
        }
        public IEnumerator FloatDuration()
        {
                _rigidbody2D.gravityScale = 0;
                yield return new WaitForSeconds(0.5f);
                _rigidbody2D.gravityScale = 2;
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

}