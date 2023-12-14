using System.Collections;
using Cinemachine;
using General.Scripts;
using StatePattern;
using UnityEngine;
using UnityEngine.UI;

namespace Entities.Player.Scripts
{
        public class PlayerController: EntityControler
        {
                // Components
                public PlayerMovementStateMachine PmStateMachine { get; private set; }
                private InputActions _controls;
                private Rigidbody2D _rigidbody2D;
                [SerializeField] private GameObject feet;
                [SerializeField] private BoxCollider2D feetBoxCollider; 
                [SerializeField] private PlayerData playerData;
                [SerializeField] private GameObject meleeAttack;
                public Animator animator;
                private SpriteRenderer _spriteRenderer;
        
        
                // internal state controls
                public bool isAirDashUnlocked = false;
                public bool isHoldingHorizontal = false;
                public bool isHoldingVertical = false;
                public bool isPerformingJump = false;
                public bool isDashing = false;
                public bool onDJump = false;
                public bool facingRight = true;
                public bool isCollidingLeft = false;
                public bool isCollidingRight = false;
                public bool isPerformingMeleeAttack = false;
                public bool isPerformingDash;
                public bool canAttack = true;
                public float attackDuration;
                public float meleeAttackCooldown;
                public float meleeAttackDuration;
                public float meleeAttackStart;
                public float jumpDuration;
        
                public float friction;
                public bool isStunned = false;
        
                // internal state variables
                public float horizontalSpeed;
                public float dashSpeed;
                public float jumpForce;
                public float baseGravity;
        
                [SerializeField] private AnimationCurve dashCurve;
                public bool onDashCooldown = false;
                public float maxAirHorizontalSpeed;
                public float maxFallSpeed;
                public float timeStunned;
        
                private AnimationCurve _dashCurve;
                private int _numberOfGrounds;
        
                [SerializeField] private float floatDuration;
        
                //UI
                private Slider healthBar;
                private Text healthText;
                private Text mainText;
        
        
                private bool _onInvulneravility;
                private Rigidbody2D _rb;
                private CapsuleCollider2D _capsule;
      
    
                public bool touchingFloor;

                private CinemachineImpulseSource _impulseSource;

                public CinemachineStateDrivenCamera cinemachine;

                private Canvas _canvasPausa;

                [SerializeField] private Audios _playerAudios;

                private AudioSource _audioSource;
        
                //Potion UI
                private bool _onPotionColdown;
                private Slider _sliderPotion;
                //Potion Prefab
                public GameObject potion;
        
                //private AudioSource _audioSource;
                void Start()
                {
                        // Audio = 
                        _audioSource = GetComponent<AudioSource>();
                        //_force2D = GetComponent<ConstantForce2D>();
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
                        _controls.GeneralActionMap.Jump.started += ctx => isPerformingJump = true;
                        _controls.GeneralActionMap.Jump.canceled += ctx => isPerformingJump = false;

                        //Dash -> Add Force in the direction the player is facing
                        _controls.GeneralActionMap.Dash.performed += ctx => isPerformingDash = true;
                        _controls.GeneralActionMap.Dash.canceled += ctx => isPerformingDash = false;
                        //MeleeAttack
                        _controls.GeneralActionMap.Attack.performed += ctx =>  isPerformingMeleeAttack = true;
                        _controls.GeneralActionMap.Attack.canceled += ctx =>  isPerformingMeleeAttack = false;
                
                
                        //Potion launch
                        _controls.GeneralActionMap.Potion.performed += ctx => Potion();
                
                        // Initialize data
                        horizontalSpeed = playerData.movementSpeed;
                        maxAirHorizontalSpeed = playerData.maxAirHorizontalSpeed;
                        _numberOfGrounds = 0;
                        _rigidbody2D.gravityScale = playerData.gravity;
                        _dashCurve = playerData.dashCurve;
                        maxFallSpeed = playerData.maxFallSpeed;
                        baseGravity = _rigidbody2D.gravityScale;
                        jumpForce = playerData.jumpPower;
                        dashSpeed = playerData.dashSpeed;
                        // Pause
                        _controls.GeneralActionMap.Pause.performed += ctx => GameController.Instance.Pause();
                
                        cinemachine = GameObject.Find("GameCameras").GetComponent<CinemachineStateDrivenCamera>();
                        healthText = GameObject.Find("TextHealth").GetComponent<Text>();
                        mainText = GameObject.Find("TextMain").GetComponent<Text>();
                        healthBar = GameObject.Find("SliderHealth").GetComponent<Slider>();
        
                        //Set health
                        _health.Set(100);
                        healthText.text = _health.Get().ToString();
                        healthBar.value = _health.Get();
        
                        //Set the rigidBody
                        _rb = GetComponent<Rigidbody2D>();
                
                        //set Potion Slider
                        _sliderPotion = GameObject.Find("SliderPotion").GetComponent<Slider>();
                        _sliderPotion.maxValue = playerData.potionColdownTime;
                        _sliderPotion.value = playerData.potionColdownTime;
                
        
                        _impulseSource = GetComponent<CinemachineImpulseSource>();
                
                
                        //Check unlocks
                        isAirDashUnlocked = playerData.airDashUnlocked;

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
                        if (!onDashCooldown && isPerformingDash) 
                        {
                                return true;
                        }

                        return false;
                }

                public void FlipSprite()
                {
                        Debug.Log("Flip");
                        float direction = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        Debug.Log(direction);
                        if (direction == -1) facingRight = false;
                        else if (direction == 1) facingRight = true;
                        _spriteRenderer.flipX = !facingRight;
                        Debug.Log(_spriteRenderer.flipX);

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

                public void AttackCooldown()
                {
                        canAttack = true;
                }
        
                public void AttackDuration()
                {
                        if (IsGrounded())
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.IdleState);
                                return;
                        }
                        PmStateMachine.TransitionTo(PmStateMachine.AirState);
                }

                public void GroundAttack()
                {
                        //aadafloat xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        float yDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();
                
                        if (yDirection == 1)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackUpState);
                                return;
                        }
                        if (facingRight)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackRightState);
                                return;
                        }
                        if (!facingRight)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackLeftState);
                                return;
                        }
                


                
                }
                public void AirAttack()
                {
                        //float xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        float yDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();
                
                        if (yDirection == 1)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackUpState);
                                return;
                        }
                        if (yDirection == -1)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackDownState);
                                return;
                        }
                        if (facingRight)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackRightState);
                                return;
                        }
                        if (!facingRight)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackLeftState);
                                return;
                        }


                
                }

                public void FinishedAttack()
                {
                        if (isHoldingHorizontal)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.WalkState);
                                return;
                        }

                        if (CanDash())
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.GroundDashState);
                                return;
                        }

                        if (isPerformingJump)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.JumpState);
                                return;
                        }

                        if (!IsGrounded())
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirState);
                
                                return;
                        }
                }
                
                public void DisablePlayerControls()
                {
                        _controls.Disable();
                }

                public void EnablePlayerControls()
                {
                        _controls.Enable();
                }
        
                void Potion()
                {
                        if (!_onPotionColdown)
                        {
                                Debug.Log("POTION LAUNCH");
                                Instantiate(potion, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
                                _sliderPotion.value = 0;
                                StartCoroutine(PotionCooldownSlider());
                        }
                }
        
                public override void OnDeath()
                {
                        Debug.Log("muerte");
                        DisablePlayerControls();
                        Invoke(nameof(CallSceneLoad), 1);
                
                        _audioSource.PlayOneShot(_playerAudios.audios[1]);
                        //_audioSource.clip = Resources.Load<AudioClip>("HITMARKER SOUND EFFECT");
                        //_audioSource.Play();
                        //_canvasPausa.gameObject.SetActive(true); 
                }
        
                public void CallSceneLoad()
                {
                        GameController.Instance.SceneLoad(GameController.Instance.GetCheckpoint(),true);
                        //gameControler.GetComponent<GameController>().SceneLoad(gameControler.GetComponent<GameController>().GetCheckpoint());
                }
        
                // -------------- COROUTINES -----------------

                public IEnumerator PotionCooldownSlider()
                {
                        _onPotionColdown = true;
                        while (_sliderPotion.value < playerData.potionColdownTime)
                        {
                                _sliderPotion.value += 0.01f;   
                                yield return new WaitForSeconds(0.01f);
                        }
                        _onPotionColdown = false;
                }
        
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
                        isPerformingJump = false;

                }

                public void setXVelocity(float i)
                {
                        _rigidbody2D.velocity = new Vector2(i, _rigidbody2D.velocity.y);
                }

                public PlayerData GetPlayerData()
                { 
                        return playerData;
                }
                private void OnCollisionEnter2D(Collision2D collision)
                {
                        //Colision con el enemigo
                        if (collision.gameObject.CompareTag("Enemy"))
                        {
                                if (!_onInvulneravility)
                                {
                                        CameraShakeManager.instance.CameraShake(_impulseSource);
                                        _audioSource.PlayOneShot(_playerAudios.audios[0]);
                                        _onInvulneravility = true;
                                        _health.RemoveHealth(25);
                                        healthText.text = _health.Get().ToString();
                                        healthBar.value = _health.Get();
                
                                        Invoke(nameof(DamageCooldown), 0.5f);
                                }
                        }
                
                        //Colision con el enemigo
                        if (collision.gameObject.CompareTag("AirDashUnlock"))
                        {
                                isAirDashUnlocked = true;
                                playerData.airDashUnlocked = true;
                                Debug.Log("AAirdash");
                        }
                }

                private void OnCollisionStay2D(Collision2D other)
                {
                        //Colision con el enemigo
                        if (other.gameObject.CompareTag("Enemy"))
                        {
                                if (!_onInvulneravility)
                                {
                                        CameraShakeManager.instance.CameraShake(_impulseSource);
                                        _onInvulneravility = true;
                                        _health.RemoveHealth(25);
                                        healthText.text = _health.Get().ToString();
                                        healthBar.value = _health.Get();
                
                                        Invoke(nameof(DamageCooldown), 0.5f);
                                }
                        }
                }
        
                public void DamageCooldown()
                {
                        _onInvulneravility = false;
                        _rb.WakeUp();
                }
        

                public void ReceiveDamage(int damage) 
                {
                        Debug.Log(_health.Get());
                        _health.RemoveHealth(damage);
                        Debug.Log(_health.Get());
                } 

                public void SetCurrentGravity(float gravity)
                {
                        _rigidbody2D.gravityScale = gravity;
                }

                public void ResetGravity()
                {
                        _rigidbody2D.gravityScale = baseGravity;
                }
        }
}