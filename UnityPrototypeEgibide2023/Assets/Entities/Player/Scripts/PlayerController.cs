using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Cinemachine;
using Entities.Potions.BasePotion.Scripts;
using General.Scripts;
using StatePattern;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = System.Numerics.Vector3;

namespace Entities.Player.Scripts
{
        public class PlayerController: EntityControler
        {
                // Components
                public PlayerMovementStateMachine PmStateMachine { get; private set; }
                private InputActions _controls;
 
                [SerializeField] private GameObject feet;
                [SerializeField] private BoxCollider2D feetBoxCollider; 
                [SerializeField] private PlayerData playerData;
                [SerializeField] public  GameObject meleeAttack;
                [SerializeField] private GameObject dashEffect;
                public Animator animator;
                [SerializeField] private SpriteRenderer _spriteRenderer;
                
        
        
                // internal state controls
                public bool isAirDashUnlocked = false;
                public bool isHoldingHorizontal = false;
                public bool isHoldingVertical = false;
                public bool isPerformingJump = false;
                public bool isPerformingMeleeAttack = false;
                public bool isPerformingDash = false;
                public bool isPerformingPotionThrow = false;
                public bool isDashing = false;
                public bool isCollidingLeft = false;
                public bool isCollidingRight = false;
                public bool isInMiddleOfAirAttack = false;
                public bool isInMiddleOfAttack = false;
                public float friction;
                public bool isStunned = false;
                public bool hasReachedSpawnPoint = false;
        
                // internal state variables
                public float horizontalSpeed;
                public float dashSpeed;
                public float jumpForce;
                public float baseGravity;
                
                [SerializeField] private AnimationCurve dashCurve;
                public bool onDashCooldown = false;
                public bool onAirDashCooldown = false;
                public float maxAirHorizontalSpeed;
                public float maxFallSpeed;
                public float timeStunned;
                public bool onHitPushback = false;
                private AnimationCurve _dashCurve;
                private int _numberOfGrounds;
        
                [SerializeField] private float floatDuration;
        
                //UI
                private Slider healthBar;
                private Text healthText;
                private Text mainText;
                
                private Rigidbody2D _rb;
                private CapsuleCollider2D _capsule;
      
    
                public bool touchingFloor;

                private CinemachineImpulseSource _impulseSource;

                public CinemachineStateDrivenCamera cinemachine;
                
                [SerializeField] private Audios _playerAudios;

                private AudioSource _audioSource;
                
                //SpawnData
                private GameController.SPlayerSpawnData _sPlayerSpawnData;
                //CurrentPersistentData
                private GameController.SPlayerPersistentData _sPlayerCurrentPersistentData;
        
                //Potion UI
                private bool _onPotionCooldown;
                private Slider _sliderPotion;
                private Image _selectedPotionImage;
                
                //Potion 
                public GameObject[] potionList;
                public GameObject selectedPotion;
                public GameObject throwPosition;
                private bool _rotated;
                private bool _onCooldown;
                private GameObject _potionSelector;


                [SerializeField] private GameObject effectSpawner;
                [SerializeField] private GameObject airDashEffect;
                //private AudioSource _audioSource;
                
                void Start()
                {

                        if (GameController.Instance.PlayerPersistentDataBetweenScenes.Equals(default(GameController.SPlayerPersistentData)))
                        {
                                _sPlayerCurrentPersistentData.CurrentHealth = 100;
                                _sPlayerCurrentPersistentData.PotionList = potionList;
                                _sPlayerCurrentPersistentData.SelectedPotion = null;
                        }
                        else
                        {
                                _sPlayerCurrentPersistentData =
                                        GameController.Instance.PlayerPersistentDataBetweenScenes;
                        }

                        // Audio = 
                        _audioSource = GetComponent<AudioSource>();
                        //_force2D = GetComponent<ConstantForce2D>();
                        animator = GetComponentInChildren<Animator>();
                        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
                        _controls = new InputActions();
                        
                        
                        //Enable the actions
                        _controls.Enable();
                
                        // Initialize the state machine
                        PmStateMachine = new PlayerMovementStateMachine(this);
                        PmStateMachine.Initialize(PmStateMachine.IdleState);
                        
                        // Subscribe to events
                        AttackComponent.OnHit += OnHit;
                        PotionBehavior.OnPotionDestroy += ResetPotionCooldown;
                        
                        
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
                        _controls.GeneralActionMap.Potion.performed += ctx => isPerformingPotionThrow = true;
                        _controls.GeneralActionMap.Potion.canceled += ctx => isPerformingPotionThrow = false;
                        //PotionChange -> Change which potion is selected
                        _controls.GeneralActionMap.ChangePotionL.performed += ctx=> ShowPotionSelector(true);
                        _controls.GeneralActionMap.ChangePotionR.performed += ctx => ShowPotionSelector(false);
                        
                        // Initialize data
                        horizontalSpeed = playerData.movementSpeed;
                        maxAirHorizontalSpeed = playerData.maxAirHorizontalSpeed;
                        _numberOfGrounds = 0;
                        Rb.gravityScale = playerData.gravity;
                        _dashCurve = playerData.dashCurve;
                        maxFallSpeed = playerData.maxFallSpeed;
                        Rb.gravityScale = playerData.gravity;
                        jumpForce = playerData.jumpPower;
                        dashSpeed = playerData.dashSpeed;
                        // Pause
                        _controls.GeneralActionMap.Pause.performed += ctx => GameController.Instance.Pause();
                
                        cinemachine = GameObject.Find("Main Camera").GetComponent<CinemachineStateDrivenCamera>();
                        healthText = GameObject.Find("TextHealth").GetComponent<Text>();
                        mainText = GameObject.Find("TextMain").GetComponent<Text>();
                        healthBar = GameObject.Find("SliderHealth").GetComponent<Slider>();
                        _selectedPotionImage = GameObject.Find("ImagePotionSelected").GetComponent<Image>();
        
                        //Set health
                        Health.Set(_sPlayerCurrentPersistentData.CurrentHealth);
                        healthText.text = Health.Get().ToString();
                        healthBar.value = Health.Get();
                
                        //set Potion Slider
                        _sliderPotion = GameObject.Find("SliderPotion").GetComponent<Slider>();
                        _sliderPotion.maxValue = playerData.potionColdownTime;
                        _sliderPotion.value = playerData.potionColdownTime;

                        _potionSelector = transform.Find("PotionSelector").gameObject;
                        potionList = _sPlayerCurrentPersistentData.PotionList;
                        
                        if(selectedPotion is null)
                                selectedPotion = _sPlayerCurrentPersistentData.SelectedPotion;
                        _selectedPotionImage.sprite = 
                                selectedPotion != null ? selectedPotion.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite : null;
                        
                        _impulseSource = GetComponent<CinemachineImpulseSource>();
                
                
                        //Check unlocks
                        isAirDashUnlocked = playerData.airDashUnlocked;
                        dashEffect.SetActive(false);
                        //CheckSceneChanged
                        OnSceneChange();
                        StartCoroutine(KeepSpriteStraight());
                }

                private void OnDestroy()
                {
                        PotionBehavior.OnPotionDestroy -= ResetPotionCooldown;
                        _controls.GeneralActionMap.ChangePotionL.performed -= ctx=> ShowPotionSelector(true);
                        _controls.GeneralActionMap.ChangePotionR.performed -= ctx => ShowPotionSelector(false);
                        _controls.Disable();
                         GameController.SPlayerPersistentData playerPersistentData =
                                 new GameController.SPlayerPersistentData();
                         
                         playerPersistentData.CurrentHealth = Health.Get() <= 0 ? 100 : Health.Get();
                         playerPersistentData.PotionList = potionList;
                         playerPersistentData.SelectedPotion = selectedPotion;
                        GameController.Instance.PlayerPersistentDataBetweenScenes = playerPersistentData;
                }

                private void ResetPotionCooldown(GameObject entity)
                {
                        StartCoroutine(PotionCooldownSlider());
                }

                private void FixedUpdate()
                {
                        //Debug.Log(IsGrounded());
                        PmStateMachine.StateUpdate();
                
                        // Clamp gravity
                        Vector2 clampVel = Rb.velocity;
                        clampVel.y = Mathf.Clamp(clampVel.y, -maxFallSpeed, 9999);
                        Rb.velocity = clampVel;
                
                }

                public void ClampVelocity(float x, float y)
                {
                        Vector2 clampVel = Rb.velocity;
                        clampVel.y = Mathf.Clamp(clampVel.y, -y, y);
                        clampVel.x = Mathf.Clamp(clampVel.x, -x, x);
                        Rb.velocity = clampVel;
                }
                
                private void OnHit(EntityControler attacker, EntityControler victim)
                {
                        // Mal pero me da pereza hacerlo mejor
                        if (attacker != this) return;
                        if (onHitPushback) return;
                        onHitPushback = true;
                        Invoke(nameof(SetOnHitPushback), 0.2f);
                        if (PmStateMachine.CurrentState == PmStateMachine.AirMeleeAttackDownState)
                        {
                                Rb.velocity = new Vector2(Rb.velocity.x, 0);
                                Rb.AddForce(new Vector2(0, 6000));
                                return;
                        }
                        if (PmStateMachine.CurrentState == PmStateMachine.AirMeleeAttackUpState)
                        {
                                Rb.velocity = new Vector2(Rb.velocity.x, 0);
                                Rb.AddForce(new Vector2(0, -6000));
                                return;
                        }
                        if (PmStateMachine.CurrentState == PmStateMachine.AirMeleeAttackForwardState)
                        {
                                Rb.velocity = new Vector2(0, Rb.velocity.y);
                                Rb.AddForce(new Vector2((this.FacingRight ?  -3000 : 3000), 0));
                                return;
                        }
                        if (PmStateMachine.CurrentState == PmStateMachine.AirMeleeAttackBackwardState)
                        {
                                Rb.velocity = new Vector2(0, Rb.velocity.y);
                                Rb.AddForce(new Vector2((this.FacingRight ?  3000 : -3000), 0));
                                return;
                        }
                        
                        if (victim is PotionBehavior) return;
                        if (PmStateMachine.CurrentState == PmStateMachine.MeleeAttackLeftState)
                        {
                                Rb.AddForce(new Vector2(1500, 0));
                                return;
                        }
                        if (PmStateMachine.CurrentState == PmStateMachine.MeleeAttackRightState)
                        {
                                Rb.AddForce(new Vector2(-1500, 0));
                                return;
                        }
                        
                }
                
                private void SetOnHitPushback()
                { 
                        onHitPushback = false;
                }



                public void Jump()
                {
                        Rb.velocity = new Vector2(Rb.velocity.x, jumpForce);
                }

                public void Move()
                {
                        FlipSprite();
                
                        if((FacingRight && isCollidingRight) || (!FacingRight && isCollidingLeft))
                        {
                                Rb.velocity = new Vector2(0, Rb.velocity.y); 
                                return;
                        }

                        Rb.velocity =
                                new Vector2((FacingRight ? horizontalSpeed : horizontalSpeed * -1), Rb.velocity.y); 

                }

                public void AirMove()
                {
                        float airAcceleration = 0.5f;
                        float airDrag = 0.01f;
                        float movementDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        if((movementDirection == 1 && isCollidingRight) || (movementDirection == -1 && isCollidingLeft))
                        {
                                Rb.velocity = new Vector2(0, Rb.velocity.y); 
                                return;
                        }

                        if (movementDirection == 1)
                        {
                                if (Rb.velocity.x > maxAirHorizontalSpeed)
                                {
                                        return;
                                }
                                Rb.velocity =
                                        new Vector2(Rb.velocity.x + airAcceleration, Rb.velocity.y);
                                return;

                        }

                        if (movementDirection == -1)
                        {
                                if (Rb.velocity.x < -maxAirHorizontalSpeed)
                                {
                                        return;
                                }

                                Rb.velocity =
                                        new Vector2(Rb.velocity.x - airAcceleration, Rb.velocity.y);
                                return;
                        }

                        if (Rb.velocity.x > 0)
                        {
                                float velocityX = Rb.velocity.x - airDrag;

                                if (velocityX < 0)
                                        velocityX = 0;
                                Rb.velocity =
                                        new Vector2(velocityX, Rb.velocity.y);
                                return;
                        }
                        
                        if (Rb.velocity.x < 0)
                        {
                                float velocityX = Rb.velocity.x + airDrag;

                                if (velocityX > 0)
                                        velocityX = 0;
                                Rb.velocity =
                                        new Vector2(velocityX, Rb.velocity.y);
                                return;
                        }


                }
 

                private IEnumerator KeepSpriteStraight()
                {
                        
                        while (true)
                        {
                               
                                int objective = FacingRight ? 0:180;
                                if ((int)_spriteRenderer.gameObject.transform.rotation.eulerAngles.y !=  objective)
                                {
                                        _spriteRenderer.gameObject.transform.eulerAngles = new UnityEngine.Vector3(_spriteRenderer.transform.transform.eulerAngles.x, _spriteRenderer.transform.rotation.eulerAngles.y + (FacingRight ? -30: 30), _spriteRenderer.transform.rotation.eulerAngles.z);

                                }
                                
                                
                                
                                yield return new WaitForSeconds(0.03f);
                        }
                }

               
        
                public bool IsGrounded()
                {
                        // if (Rb.velocity.y > 0) return false;
                        return 0 < _numberOfGrounds;
                }

                public void ThrowPotion()
                {
                        _sliderPotion.value = 0;
                        _onPotionCooldown = true;
                       Instantiate(selectedPotion, 
                               new Vector2(throwPosition.transform.position.x + (this.FacingRight ? 0.3f : -0.3f), throwPosition.transform.position.y), 
                               Quaternion.identity)
                               .GetComponent<Rigidbody2D>().velocity = new Vector2(
                               ((!isHoldingVertical ? (this.FacingRight ? 1 : -1) : 0) * 5) + Rb.velocity.x,
                               5);
                }

                public void ThrowPotionAir()
                {
                        _sliderPotion.value = 0;
                        _onPotionCooldown = true;
                        float vDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();
                        float xPos = throwPosition.transform.position.x + (this.FacingRight ? 0.3f : -0.3f);
                        float yPos = throwPosition.transform.position.y + (vDirection == -1 ? -1.5f : 0f);
                        GameObject potion = Instantiate(selectedPotion,
                                new Vector2(xPos + (this.FacingRight ? 0.3f : -0.3f), yPos),
                                Quaternion.identity);
                        
                        potion.GetComponent<Rigidbody2D>().velocity = new Vector2(
                                ((!isHoldingVertical ? (this.FacingRight ? 1 : -1) : 0) * 5) + Rb.velocity.x, (!isHoldingVertical ? 0.5f : vDirection) * 5);
                }
                
                

                public void EndThrowPotion()
                {
                        if (isHoldingHorizontal)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.WalkState);
                                return;
                        }
                        
                        PmStateMachine.TransitionTo(PmStateMachine.IdleState);
                }

                public void EndThrowPotionAir()
                {
                        PmStateMachine.TransitionTo(PmStateMachine.AirborneState);
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
                
                public bool CanAirDash()
                {
                        if (!onAirDashCooldown && isPerformingDash && isAirDashUnlocked) 
                        {
                                return true;
                        }

                        return false;
                }

                public bool CanThrowPotion()
                {
                        return !_onPotionCooldown && isPerformingPotionThrow;
                }

                public void FlipSprite()
                {
                        float direction = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        if (direction == -1)
                        {
                                FacingRight = false;
                              
                        }
                        else if (direction == 1)
                        {
                                FacingRight = true;
                                
                        }
                        

                }
                
                
                
                
                
                public void AirDash()
                {
                        float xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        float yDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();

                        float xForce = playerData.airdashForce * (xDirection < 0 ? -1 : 1);
                        float yForce = playerData.airdashForce * (yDirection < 0 ? -1 : 1);

                        Rb.velocity = new Vector2(xForce, yForce * 2);


                }

                public void GroundAttack()
                {
                        //aadafloat xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        float yDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();
                
                        float xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        
                        if (yDirection == 1)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackUpState);
                                return;
                        }
                        if (xDirection == 1)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackRightState);
                                return;
                        }
                        if (xDirection == -1)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackLeftState);
                                return;
                        }
                        
                        if (FacingRight)
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackRightState);
                                return;
                        }
                        PmStateMachine.TransitionTo(PmStateMachine.MeleeAttackLeftState);

                        
                


                
                }
                public void AirAttack()
                {
                        // TODO: ARREGLAR IZQUIERDA DERECHA
                        //float xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
                        float yDirection = _controls.GeneralActionMap.VerticalMovement.ReadValue<float>();
                        float xDirection = _controls.GeneralActionMap.HorizontalMovement.ReadValue<float>();
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
                        if ((xDirection == 1 && FacingRight) || (xDirection == -1 && !FacingRight))
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackForwardState);
                                return;
                        }
                        if ((xDirection == -1 && FacingRight) || (xDirection == 1 && !FacingRight))
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackBackwardState);
                                return;
                        }
                        
                        PmStateMachine.TransitionTo(PmStateMachine.AirMeleeAttackForwardState);
                }

                public void EndAttack()
                {
                        /* if (!IsGrounded())
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirState);
                
                                return;
                        }
                        
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
                        
                        PmStateMachine.TransitionTo(PmStateMachine.IdleState); */
                        isInMiddleOfAttack = false;
                }

                public void EndAirAttack()
                {
                        /* 
                        if (!IsGrounded())
                        {
                                PmStateMachine.TransitionTo(PmStateMachine.AirState);
                                return;
                        }
                        
                        PmStateMachine.TransitionTo(PmStateMachine.IdleState);
                        */
                        isInMiddleOfAirAttack = false;
                        
                }
                
                public void DisablePlayerControls()
                {
                        _controls.Disable();
                }

                public void EnablePlayerControls()
                {
                        _controls.Enable();
                }

                /*Changes the selected potion and show the potions while changing them*/
                private void ChangePotion(bool leftwards)
                {
                        //TODO -> Get Selected Potion (Maybe this on Scene change maybe perhaps)
                        int selectedIndex = -1;
                        int leftIndex = -1;
                        int rightIndex = -1;
                        int valueChange = leftwards ? -1 : 1;//Joder! Un Operador Ternario
                        
                        for (var i = 0; i < potionList.Length; i++)
                        {
                                if (potionList[i] == selectedPotion)
                                {
                                        selectedIndex = i;
                                        break;
                                }
                        }

                        selectedIndex += valueChange;
                        if (selectedIndex >= potionList.Length) selectedIndex = 0;
                        if (selectedIndex < 0) selectedIndex = potionList.Length - 1;
                        
                        //Put the selected potion index as center image, and change the flowers image
                        selectedPotion = potionList[selectedIndex];
                        _potionSelector.transform.Find("SelectedPotionSprite").gameObject.GetComponent<SpriteRenderer>().sprite =
                                selectedPotion.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
                        
                        _selectedPotionImage.sprite = selectedPotion.transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
                        
                        if (potionList.Length > 1)
                        {
                                leftIndex = selectedIndex - 1;
                                if (leftIndex >= potionList.Length) leftIndex = 0;
                                if (leftIndex < 0) leftIndex = potionList.Length - 1;
                                var leftPotionSprite = _potionSelector.transform.Find("LeftPotionSprite").gameObject;
                                if(!leftPotionSprite.activeSelf) leftPotionSprite.SetActive(true);
                                
                                leftPotionSprite.GetComponent<SpriteRenderer>().sprite =
                                        potionList[leftIndex].transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
                        }

                        if (potionList.Length > 2)
                        {
                                rightIndex = selectedIndex + 1;
                                if (rightIndex >= potionList.Length) rightIndex = 0;
                                if (rightIndex < 0) rightIndex = potionList.Length - 1;
                                var rightPotionSprite = _potionSelector.transform.Find("RightPotionSprite").gameObject;
                                if(!rightPotionSprite.activeSelf) rightPotionSprite.SetActive(true);

                                rightPotionSprite.GetComponent<SpriteRenderer>().sprite =
                                        potionList[rightIndex].transform.Find("Sprite").gameObject.GetComponent<SpriteRenderer>().sprite;
                        }

                }

                private void ShowPotionSelector(bool leftwards)
                {
                        CancelInvoke(nameof(HidePotionSelector));
                        Invoke(nameof(HidePotionSelector),0.5f);
                        _potionSelector.SetActive(true);
                        ChangePotion(leftwards);
                }

                private void HidePotionSelector()
                {
                        _potionSelector.SetActive(false);
                }


                public override void OnDeath()
                {
                        DisablePlayerControls();
                        GameController.Instance.GameOver();
                        GameController.Instance.justDied = true;
                        //Invoke(nameof(CallSceneLoad), 1);
                        //_audioSource.PlayOneShot(_playerAudios.audios[1]);

                }
                

        
                public void CallSceneLoad()
                {
                        GameController.Instance.SceneLoad(GameController.Instance.GetCheckpoint(),true);
                        //gameController.GetComponent<GameController>().SceneLoad(gameController.GetComponent<GameController>().GetCheckpoint());
                }

                /*Saves the playerSpawnData so that it can be moved to it's starting position*/
                public void CheckSceneChanged()
                {
                        _sPlayerSpawnData = GameController.Instance.PlayerSpawnDataInNewScene;
                }
                
                /*Transitions to SceneChangeState which handles player spawn in new scene*/
                private void OnSceneChange()
                {
                        if(!GameController.Instance.justDied) PmStateMachine.TransitionTo(PmStateMachine.SceneChangeState);
                        GameController.Instance.justDied = false;
                }
                
                /*
                 * Forces movement of the player when no input is provided and
                 * it is needed to change its position
                 */
                public void ForceMove()
                {
                        
                        UnityEngine.Vector3 tempVector3 = transform.position;
                        float moveStep = 0.1f;
                        
                        /*Mira que rico el operador ternario Alejandro :) <3*/
                        tempVector3.x += (FacingRight ? moveStep : -moveStep);
                        /*Bua increible lo has visto bien? que bonico UwU*/
                        gameObject.transform.position = tempVector3;
  
                }
                // -------------- COROUTINES -----------------

                public IEnumerator PotionCooldownSlider()
                {
                        while (_sliderPotion.value < playerData.potionColdownTime)
                        {
                                _sliderPotion.value += 0.01f;   
                                yield return new WaitForSeconds(0.01f);
                        }
                        _onPotionCooldown = false;
                }
        
                public IEnumerator Dash()
                {
                        float dashTime = 0;
                        float dashSpeedCurve = 0;
                        while (dashTime < _dashCurve.keys[_dashCurve.length - 1].time)
                        {
                                dashSpeedCurve = _dashCurve.Evaluate(dashTime) * dashSpeed; 
                                Rb.velocity = new Vector2((FacingRight ? dashSpeedCurve : dashSpeedCurve * -1), 0);
                                if (isPerformingJump) break;
                                yield return new WaitForSeconds(Time.deltaTime); 
                                dashTime += Time.deltaTime;
                        }
                        isDashing = false;
                }

                public IEnumerator AirDashDuration()
                {
                        Vector2 dashDirection = new Vector2(_controls.GeneralActionMap.HorizontalMovement.ReadValue<float>(), _controls.GeneralActionMap.VerticalMovement.ReadValue<float>());
                        Rb.velocity = new Vector2(dashDirection.x * playerData.airdashForce, dashDirection.y * playerData.airdashForce);
                        yield return new WaitForSeconds(playerData.airdashDuration);
                        PmStateMachine.TransitionTo(PmStateMachine.AirborneState);
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
                public IEnumerator DashCooldown()
                {
                        onDashCooldown = true;
                        _spriteRenderer.material.EnableKeyword("");
                        yield return new WaitForSeconds(playerData.dashCooldown);
                        _spriteRenderer.material.DisableKeyword("");
                        onDashCooldown = false;
                        dashEffect.SetActive(true);
                        Invoke(nameof(DisableDashEffect), 0.3f);
                }
                
                private void DisableDashEffect()
                {
                        dashEffect.SetActive(false);
                }
                
                public IEnumerator AirDashCooldown()
                {
                        onAirDashCooldown = true;
                        while (!IsGrounded())
                        { 
                                yield return new WaitForFixedUpdate();
                        }
                        onAirDashCooldown = false;
                }

                public void AirDashEffect()
                {
                        Instantiate(airDashEffect,
                                new Vector2(effectSpawner.transform.position.x,
                                        effectSpawner.transform.position.y),
                                Quaternion.identity);
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
                        Rb.velocity = new Vector2(i, Rb.velocity.y);
                }

                public void SetYVelocity(float i)
                {
                        Rb.velocity = new Vector2(Rb.velocity.x, i);
                }

                public void RestartGravity()
                {
                        Rb.gravityScale = playerData.gravity;
                }
        
                public void SetGravity(float i)
                {
                        Rb.gravityScale = i;
                }

                public GameController.SPlayerSpawnData GetSPlayerSpawnData()
                {
                        return _sPlayerSpawnData;
                }

                public void SetSPlayerSpawnData(GameController.SPlayerSpawnData spsd)
                {
                        _sPlayerSpawnData = spsd;
                }

                // --------------- EVENTS ----------------------

                public IEnumerator MaxJumpDuration()
                {
                        yield return new WaitForSeconds(playerData.jumpDuration);
                        isPerformingJump = false;

                }
                

                public PlayerData GetPlayerData()
                { 
                        return playerData;
                }
                
                
                private void OnCollisionEnter2D(Collision2D collision)
                {
                        //Colision con el enemigo
                        /* if (collision.gameObject.CompareTag("Enemy"))
                        {
                                CameraShakeManager.instance.CameraShake(_impulseSource);
                                _audioSource.PlayOneShot(_playerAudios.audios[0]);
                                OnReceiveDamage(25);
                        } */
                
                        //Colision con el enemigo
                        if (collision.gameObject.CompareTag("AirDashUnlock"))
                        {
                                isAirDashUnlocked = true;
                                playerData.airDashUnlocked = true;
                                
                        }
                }

                private void OnCollisionStay2D(Collision2D other)
                {
                        //Colision con el enemigo
                        /* if (other.gameObject.CompareTag("Enemy"))
                        {
                        CameraShakeManager.instance.CameraShake(_impulseSource);
                        OnReceiveDamage(25);
                        } */
                }
                private IEnumerator CoInvulnerability()
                {
                        _spriteRenderer.material.EnableKeyword("HITEFFECT_ON");
                        while (Invulnerable)
                        {
                                _spriteRenderer.material.SetFloat("_Alpha", 0.3f);
                                
                                yield return new WaitForSeconds(0.02f);
                                _spriteRenderer.material.SetFloat("_Alpha", 1f);
                                yield return new WaitForSeconds(0.05f);
                        }
                        _spriteRenderer.material.SetFloat("_Alpha", 1f);
                        _spriteRenderer.material.DisableKeyword("HITEFFECT_ON");
                        yield return null;
                }
                public void EnableInput()
                {
                        _controls = new InputActions();
                }
                public override void OnReceiveDamage(AttackComponent.AttackData attack, bool facingRight = true)
                {
                        if (attack.damage > 10) PmStateMachine.TransitionTo(PmStateMachine.StunnedState);
                        base.OnReceiveDamage(attack, facingRight);
                        StartCoroutine(CoInvulnerability());
                        healthText.text = Health.Get().ToString();
                        healthBar.value = Health.Get();
                } 

                public void SetCurrentGravity(float gravity)
                {
                        Rb.gravityScale = gravity;
                }

                public void ResetGravity()
                {
                        Rb.gravityScale = baseGravity;
                }
                
                public void SetIsOnMiddleOfAirAttack()
                {
                        isInMiddleOfAirAttack = true;
                }

                public void SetOutOfDashVelocity()
                {
                        Rb.velocity = new Vector2((FacingRight ? 15f : -15f), Rb.velocity.y);
                }
        }
}