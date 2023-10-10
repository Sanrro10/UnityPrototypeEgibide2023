using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : EntityControler
{
    public Text healthText;
    public Text mainText;
    
    // Start is called before the first frame update
    void Start()
    {
        _health.Set(100);
        healthText.text = _health.Get().ToString();
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
            _health.RemoveHealth(25);
            healthText.text = _health.Get().ToString();
        }
    }

    public override void OnDeath() 
    {
        base.OnDeath(); //The code in Entity Controler
        Time.timeScale = 0f;
        mainText.text = "Game over";
        Debug.Log("Game over");
    }
}
