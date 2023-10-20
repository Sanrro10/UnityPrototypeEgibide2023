using System;
using System.Collections;
using System.Collections.Generic;
using StatePattern;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : EntityControler
{
    public Text healthText;
    public Text mainText;
    private bool _onInvulneravility;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsule;
    public Slider healthBar;
    
    public bool touchingFloor;
    private GameObject _elTodo;
    
    // Start is called before the first frame update
    void Start()
    {
        _health.Set(100);
        healthText.text = _health.Get().ToString();
        healthBar.value = _health.Get();
        //_rb = GetComponent<Rigidbody2D>();
        
        _elTodo = GameObject.Find("ElTodo");
    }
    
    private void Update()
    {
        /*
         * JMG
         * Check if the onAir Variable in the PlayerMovement Script has changed.
         * This is to check if the feet have touched the floor.
         */

        //touchingFloor = _elTodo.GetComponent<PlayerMovement>()
            

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Colision con el enemigo
        if (collision.gameObject.tag == "Enemy")
        {
            if (!_onInvulneravility)
            {
                _onInvulneravility = true;
                _health.RemoveHealth(25);
                healthText.text = _health.Get().ToString();
                healthBar.value = _health.Get();
                
                Invoke(nameof(DamageCooldown), 0.5f);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        //Colision con el enemigo
        if (other.gameObject.tag == "Enemy")
        {
            if (!_onInvulneravility)
            {
                _onInvulneravility = true;
                _health.RemoveHealth(25);
                healthText.text = _health.Get().ToString();
                healthBar.value = _health.Get();
                
                Invoke(nameof(DamageCooldown), 0.5f);
            }
        }
    }

    public override void OnDeath() 
    {
        base.OnDeath(); //The code in Entity Controler
        Time.timeScale = 0f;
        mainText.text = "Game over";
        Debug.Log("Game over");
    }

    public void DamageCooldown()
    {
        _onInvulneravility = false;
        //_rb.WakeUp();
    }
    
    /**
     * Check if the Feet of the player have touched the floor
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.tag == "Floor")
        {
            touchingFloor = true;
            _elTodo.SendMessage("ResetJump");
        }
        
    }


}
