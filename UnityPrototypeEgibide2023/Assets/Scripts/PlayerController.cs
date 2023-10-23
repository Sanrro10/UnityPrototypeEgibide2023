using StatePattern;
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
        
        [SerializeField] private PlayerData playerData;
        void Start()
        {
                animator = GetComponent<Animator>();
                _spriteRenderer = GetComponent<SpriteRenderer>();
                _controls = new InputActions();
                _numberOfGrounds = 0;
                //Enable the actions
                _controls.Enable();

                pmStateMachine.Initialize(pmStateMachine.IdleState);
                //Inputs
                _controls.GeneralActionMap.Movement.started += ctx => isMoving = true;
                _controls.GeneralActionMap.Movement.canceled += ctx => isMoving = false;

                //Jump
                _controls.GeneralActionMap.Jump.performed += ctx => isJumping = true;

                //Dash -> Add Force in the direction the player is facing
                _controls.GeneralActionMap.Dash.performed += ctx => isDashing = true;
                
        }

        private void FixedUpdate()
        {
                pmStateMachine.Update();
                CalculateVertical();
                Move();
        }

        private void CalculateVertical()
        {
                
        }
        
        

        private void Move()
        {
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
        
        // --------------- EVENTS ----------------------
        private void OnTriggerEnter2D(Collider2D collision)
        {
                if (collision.gameObject.tag.Equals("Floor"))
                {
                        _numberOfGrounds++;
                }
        
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
                if (collision.gameObject.tag.Equals("Floor"))
                {
                        _numberOfGrounds--;
                }
        }
}