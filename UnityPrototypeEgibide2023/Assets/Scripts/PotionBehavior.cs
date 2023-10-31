using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{


    [SerializeField] private float verticalSpeed;

    [SerializeField] private int health;
    
    [SerializeField] private GameObject owner;
    [SerializeField] private GameObject explosion;
    void Start()
    {

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
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1 * verticalSpeed );
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
