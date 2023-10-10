using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    public HealthComponent health;
    public Text healthText;
    
    // Start is called before the first frame update
    void Start()
    {
        health.Set(100);
        healthText.text = health.Get().ToString();
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
            health.RemoveHealth(25);
            healthText.text = health.Get().ToString();
        }
    }
}
