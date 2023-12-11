using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Serialization;

public class PotionBehavior : MonoBehaviour
{

    private PlayerController _playerController;   
    private GameObject _player;



    [SerializeField] private float _speed;

    [SerializeField] private int health;
    
    [SerializeField] private GameObject owner;
    [SerializeField] private GameObject explosion;
    void Start()
    {
        _player = GameObject.Find("Player Espada State");
        _playerController = _player.GetComponent<PlayerController>();
        
        
        ApplyForce();
        
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // if collision is against floor 
         if (collision.gameObject.CompareTag("Enemy"))
        {
                Explode();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
                Explode();
        } 
        else
        {
            Bounce(1);
        }
        
        
        
    }

    private void ApplyForce()
    {
        if (_playerController.facingRight)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2( 1 * _speed, 0 );
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2( -1 * _speed, 0 );
        }
        
        
        
    }

    
    private void Bounce(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Explode();
        }
    }
    

    private void Explode()
    {   
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    
    public void SetOwner(GameObject owner)
    {
        owner = owner;
    }
    
    public GameObject GetOwner()
    {
        return owner;
    }
}
