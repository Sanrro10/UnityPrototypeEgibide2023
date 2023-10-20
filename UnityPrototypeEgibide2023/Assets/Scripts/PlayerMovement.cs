using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputActions _controls;
    private bool _onAir;
    private bool _onDoubleJump;
    private bool _onDownAttack;
    private bool _onDash;
    private bool _onDashCooldown;
    
    [SerializeField] private PlayerData playerData;
    
    // Start is called before the first frame update
    void Start()
    {
        _controls = new InputActions();

        //Enable the actions
        _controls.Enable();

        //Movement
        _controls.GeneralActionMap.Movement.started += ctx => InvokeRepeating(nameof(Move), 0, 0.01f);
        _controls.GeneralActionMap.Movement.canceled += ctx => CancelInvoke(nameof(Move));

        //Jump
        _controls.GeneralActionMap.Jump.performed += ctx => Jump();

        //Dash -> Add Force in the direction the player is facing
        _controls.GeneralActionMap.Dash.performed += ctx => Dash();
    }

    void Move()
    {
        if (!_onDash)
        {

            Vector2 direccion = _controls.GeneralActionMap.Movement.ReadValue<Vector2>();

            if (direccion.Equals(Vector2.left) || direccion.Equals(Vector2.right))
            {
                playerData.facingRight = direccion.x == 1 ? true : false;
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

    void Jump()
    {
        if (!_onDownAttack)
        {
            if (!_onAir)
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.up * playerData.jumpPower;
                _onAir = true;
                _onDoubleJump = false;
            }
            else if (!_onDoubleJump)
            {
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

    public void ControlsDissable()
    {
        _controls.Disable();
    }
    
    // --------------- EVENTS ----------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.tag == "Floor")
        {
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

}
