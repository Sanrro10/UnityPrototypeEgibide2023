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
        public float horizontalSpeed;
        private float _verticalSpeed;
        private float _jumpForce;
        private float _currentGravity { get; set; }
        public Animator animator;
        private SpriteRenderer _spriteRenderer;
        private int _numberOfGrounds;
        private Rigidbody2D _rigidbody2D;
        
        [SerializeField] private PlayerData playerData;
        void Start()
        {
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
                pmStateMachine.StateUpdate();
                
        }

        private void CalculateVertical()
        {
                
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
                transform.position += new Vector3((facingRight ? horizontalSpeed : horizontalSpeed * -1), 0, 0);
        }
        
        public bool IsGrounded()
        {
                return 0 < _numberOfGrounds;
        }

        public void SetCurrentGravity(float gravity)
        {
                this._currentGravity = gravity;
        }

        public void ResetGravity()
        {
                this._currentGravity = playerData.gravity;
        }
        
        public void Dash()
        {
                float dashValue = (playerData.movementSpeed * 100) * playerData.dashSpeed;
                dashValue *= (facingRight ? 1 : -1);
                _rigidbody2D.velocity = Vector2.right * dashValue;
                _rigidbody2D.gravityScale = 0;
        }
        
        
        // -------------- COROUTINES -----------------
        public IEnumerator DashDuration()
        {
                yield return new WaitForSeconds(playerData.dashDuration);
                _rigidbody2D.velocity = Vector2.zero;
                _rigidbody2D.gravityScale = 2;
                pmStateMachine.TransitionTo(pmStateMachine.IdleState);
        }
        
        // --------------- EVENTS ----------------------
        private void OnCollisionEnter2D(Collision2D collision)
        {
                if (collision.gameObject.tag.Equals("Floor"))
                {
                        _numberOfGrounds++;
                }
        
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
                if (collision.gameObject.tag.Equals("Floor"))
                {
                        _numberOfGrounds--;
                }
        }
}