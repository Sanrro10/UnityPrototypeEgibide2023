using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    //Este script gestiona las partes del combate cuerpo a cuerpo - Incluida la posicion de la espada
    
    private InputActions _controls;

    private Animator _animator;
    
    void Start()
    {
        _controls = new InputActions();
        _animator = GameObject.Find("ElTodo").GetComponent<Animator>();
        
        _controls.Enable();
       
        //Melee Attack
        _controls.GeneralActionMap.Attack.performed += ctx => Attack();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        if (this._animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Debug.Log("ATACOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            _animator.SetTrigger("Attack");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("LE HE DADO");
            //Logica de Dañar enemigos
            
        }
    }
}
