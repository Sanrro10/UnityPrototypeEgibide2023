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

    }

    void Move()
    {
        float movementValue = _controls.GeneralActionMap.Movement.ReadValue<float>();
        transform.position += new Vector3(playerData.movementSpeed * movementValue, 0, 0);
    }

    void Jump()
    {
        if (!_onAir)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.up * playerData.jumpPower;
            _onAir = true;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            _onAir = false;
        }
    }
}
