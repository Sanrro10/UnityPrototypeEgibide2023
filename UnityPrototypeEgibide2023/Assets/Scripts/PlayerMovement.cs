using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputActions _controls;
    private Boolean _onAir;
    private Boolean _onDoubleJump;
    private Boolean _onDownAttack;
    
    [SerializeField] private PlayerData playerData;
    private Rigidbody2D playerBody;

    private bool facingRight;
    
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
        playerBody = GetComponent<Rigidbody2D>();
        _controls.GeneralActionMap.Dash.performed += ctx => Dash();
    }

    void Move()
    {
        Vector2 direccion = _controls.GeneralActionMap.Movement.ReadValue<Vector2>();
        
        if (direccion.Equals(Vector2.left) || direccion.Equals(Vector2.right))
        {
            transform.position += new Vector3(playerData.movementSpeed * direccion.x, 0, 0);
        }

        if (direccion.Equals(Vector2.down) && _onAir && !_onDownAttack)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * playerData.downAttack;
            _onDownAttack = true;
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
        float dashValue = _controls.GeneralActionMap.Movement.ReadValue<float>() * 160;
        dashValue *= (playerData.facingRight ? 1 : -1);
        playerBody.AddForce(transform.right * dashValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.GetType() == typeof(BoxCollider2D))
        {
            if (collision.gameObject.tag == "Floor")
            {
                _onAir = false;
                _onDoubleJump = false;
                _onDownAttack = false;
            }
        }
        //When gets it in the body (for combat)
        //else if (collision.otherCollider.GetType() == typeof(CapsuleCollider2D)) {}
      
    }
}
