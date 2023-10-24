using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StatePattern;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : EntityControler
{
    private Text healthText;
    private Text mainText;
    private bool _onInvulneravility;
    private Rigidbody2D _rb;
    private CapsuleCollider2D _capsule;
    private Slider healthBar;
    
    public bool touchingFloor;
    private GameObject _elTodo;

    private CinemachineImpulseSource _impulseSource;
    
    private GameObject gameControler;
    
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
        
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        
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
                CameraShakeManager.instance.CameraShake(_impulseSource);
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
        GetComponent<PlayerMovement>().DisablePlayerControls();
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
    
    /**
     * Check if the Feet of the player have touched the floor
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
      
        if (collision.gameObject.tag == "Floor")
        {
            touchingFloor = true;
            
        }
        
    }


}
