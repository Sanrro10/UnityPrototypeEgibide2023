using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private InputActions _controls;
    private bool facingRight = true;
    
    
    public Animator _animator;
    private SpriteRenderer _spriteRenderer;
    
    private bool _onAir;
    private bool _onDoubleJump;
    private bool _onDownAttack;
    private bool _onDash;
    private bool _onDashCooldown;
    private bool _touchingFloor;
    
    [SerializeField] private PlayerData playerData;
    
    private GameObject _player;

    private Canvas _canvasPause;
    
    // Start is called before the first frame update

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _controls = new InputActions();
    }

    void Start()
    {
        // _animator = GetComponent<Animator>();
        // _spriteRenderer = GetComponent<SpriteRenderer>();
        // _controls = new InputActions();
        
        
        
        //Enable the actions
        _controls.Enable();

        //Movement
        _controls.GeneralActionMap.Movement.started += ctx => InvokeRepeating(nameof(Move), 0, 0.01f);
        _controls.GeneralActionMap.Movement.canceled += ctx => CancelMoving();

        //Jump
        _controls.GeneralActionMap.Jump.performed += ctx => Jump();

        //Dash -> Add Force in the direction the player is facing
        _controls.GeneralActionMap.Dash.performed += ctx => Dash();
        
        //Pause
        _controls.GeneralActionMap.Pause.performed += ctx => Pause();
        
        //Take the player item
        _player = GameObject.Find("Player");
    }
    
    private void Update()
    {
        /*
         * JMG
         * Check if the touchingFloor variable in the PlayerController Script has changed.
         * This is to check if the feet have touched the floor
         */

        //_touchingFloor = _player.GetComponent<PlayerControler>().touchingFloor;
    }


    void Move()
    {
        _animator.SetBool("IsMoving", true);
        if (!_onDash)
        {
            Vector2 direccion = _controls.GeneralActionMap.Movement.ReadValue<Vector2>();

            if (direccion.Equals(Vector2.left) || direccion.Equals(Vector2.right))
            {
                facingRight = direccion.x == 1 ? true : false;
                _spriteRenderer.flipX = !facingRight;
                transform.position += new Vector3(playerData.movementSpeed * direccion.x, 0, 0);
            }

            if (direccion.Equals(Vector2.down) && _onAir && !_onDownAttack)
            {
                GetComponent<Rigidbody2D>().gravityScale = 2;
                GetComponent<Rigidbody2D>().velocity = Vector2.down * playerData.downAttack;
                _onDownAttack = true;
            }
        }
    }

    void CancelMoving()
    {
        CancelInvoke(nameof(Move));
        _animator.SetBool("IsMoving", false);

    }

    void Jump()
    {
        if (!_onDownAttack)
        {
            if (!_onAir)
            {
                _animator.SetBool("OnAir", true);
                _animator.SetTrigger("Jump");
                GetComponent<Rigidbody2D>().velocity = Vector2.up * playerData.jumpPower;
                _onAir = true;
                _onDoubleJump = false;
            }
            else if (!_onDoubleJump)
            {
                _animator.SetTrigger("Jump");
                GetComponent<Rigidbody2D>().velocity = Vector2.up * playerData.jumpPower;
                _onDoubleJump = true;
            }
        }
    }
    

    void Dash()
    {
        if (!_onDash || !_onDashCooldown)
        {
            _onDashCooldown = true;
            _onDash = true;
            _animator.SetTrigger("Dash");
            StartCoroutine(dashDuration());
            float dashValue = (playerData.movementSpeed * 100) * playerData.dashSpeed;
            dashValue *= (playerData.facingRight ? 1 : -1);
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().velocity = Vector2.right * dashValue;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            Invoke(nameof(DashStop), playerData.dashCooldown);
        }
    }

    void DashStop()
    {
        _onDashCooldown = false;
        _onDash = false;
    }


    /*public void ResetJump()
    {
        _onAir = false;
        _onDoubleJump = false;
        _onDownAttack = false;
    }*/
    // --------------- EVENTS ----------------------
    
     private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "Floor")
        {
            _animator.SetBool("OnAir", false);
            _onAir = false;
            _onDoubleJump = false;
            _onDownAttack = false;
        }
        
    }
    
    
    // -------------- COROUTINES -----------------
    private IEnumerator dashDuration()
    {
        yield return new WaitForSeconds(playerData.dashDuration);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        GetComponent<Rigidbody2D>().gravityScale = 2;
    }

    public void DisablePlayerControls()
    {
        _controls.Disable();
    }

    public void EnablePlayerControls()
    {
        _controls.Enable();
    }

    public void Pause()
    {
        RespawnManager.respawnManagerInstance.Pause();
        DisablePlayerControls();
    }

}
