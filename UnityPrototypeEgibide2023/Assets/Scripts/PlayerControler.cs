using System;
using System.Collections;
using System.Collections.Generic;
using StatePattern;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : EntityControler
{

    private bool _onInvulneravility;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsule;

    
    private GameObject gameControler;
    private Text healthText;
    private Text mainText;
    private Slider healthBar;
    private Canvas canvas;
    
    // Start is called before the first frame update
    void Start()
    {

        gameControler = GameObject.Find("GameControler");
        healthText = GameObject.Find("TextHealth").GetComponent<Text>();
        mainText = GameObject.Find("TextMain").GetComponent<Text>();
        healthBar = GameObject.Find("SliderHealth").GetComponent<Slider>();
        
        
        //Set health
        _health.Set(100);
        healthText.text = _health.Get().ToString();
        healthBar.value = _health.Get();
        
        //Set the rigidBody
        _rb = GetComponent<Rigidbody2D>();
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
        PlayerDeath();
        //base.OnDeath(); //The code in Entity Controler
        //Time.timeScale = 0f;
       // mainText.text = "Game over";
        //Debug.Log("Game over");
    }

    public void PlayerDeath()
    {
        //gameControler.GetComponent<RespawnManager>().PlayerRespawn();
        GetComponent<PlayerMovement>().ControlsDissable();
        Invoke(nameof(CallSceneLoad), 1);
    }

    public void CallSceneLoad()
    {
        gameControler.GetComponent<RespawnManager>().SceneLoad();
    }

    public void DamageCooldown()
    {
        _onInvulneravility = false;
        _rb.WakeUp();
    }
}
