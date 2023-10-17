using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : EntityControler
{
    public Text healthText;
    public Text mainText;
    private bool _onInvulneravility;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsule;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _health.Set(100);
        healthText.text = _health.Get().ToString();
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
        _rb.WakeUp();
    }
}
