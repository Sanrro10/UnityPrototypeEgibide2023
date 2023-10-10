using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    
    private InputActions _controls;

    private Animator animator;
    
    void Start()
    {
        _controls = new InputActions();
        animator = GetComponent<Animator>();
        
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
        if (this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            Debug.Log("ATACOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            animator.SetTrigger("MeleeAttack");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("LE HE DADO");
        }
    }
}
