using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private InputActions _controls;
    [SerializeField] private PlayerData playerData;
    
    // Start is called before the first frame update
    void Start()
    {
        _controls = new InputActions();

        //Enable the actions
        _controls.Enable();
        
        //_controls.GeneralActionMap.Movement.started += ctx => StartMove();
    }

    // void StartMove()
    // {
    //     StopCoroutine(Move());
    //     StartCoroutine(Move());
    // }
    // IEnumerator Move()
    // {
    //     while (_controls.GeneralActionMap.Movement.IsPressed())
    //     {
    //         float movementValue = _controls.GeneralActionMap.Movement.ReadValue<float>();
    //         transform.position += new Vector3(playerData.movementSpeed * movementValue, 0,0);
    //         yield return 0.01;
    //     }
    //     yield return null;
    // }

    private void FixedUpdate()
    {
        float movementValue = _controls.GeneralActionMap.Movement.ReadValue<float>();
        transform.position += new Vector3(playerData.movementSpeed * movementValue, 0,0);
    }
}
