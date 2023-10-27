using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyscript : EntityControler
{
    [SerializeField] private float flyingSpeed;

    [SerializeField] private GameObject player;

    [SerializeField] private FlyingEnemyData flyingEnemyData;

    private Vector3 _origin;
    private bool _followPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        _origin = transform.position;
        
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set",flyingEnemyData.health, SendMessageOptions.RequireReceiver);
    }

    // Update is called once per frame
    void Update()
    {
        if (_followPlayer)
        {
            Vector3 direction = player.transform.position - transform.position;
            direction.Normalize(); 
            transform.Translate(direction * (flyingSpeed * Time.deltaTime));
        }
        else
        {
            Vector3 direction = _origin - transform.position;
            direction.Normalize(); 
            transform.Translate(direction * ((float)(flyingSpeed * 1.5) * Time.deltaTime));
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Siguiendo a player");
            _followPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Dejando de seguir a player");
            _followPlayer = false;
        }
    }
    
    public override void OnDeath()
    {
        //TO-DO Activar animacion de muerte y logica relacionada
        
        Invoke(nameof(DestroyThis),2f);
        
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }
    
}
