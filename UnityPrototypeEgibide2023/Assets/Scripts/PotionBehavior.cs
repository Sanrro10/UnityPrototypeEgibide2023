using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionBehavior : MonoBehaviour
{

    // Start is called before the first frame update
    [SerializeField] private float horizontalSpeed;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private int horizontalDirection;
    [SerializeField] private int verticalDirection;
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
        if (collision.gameObject.CompareTag("Wall"))
        {
            
            return;
        }
        
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject != owner)
            {
                Bounce(99);
            }
        }
        else if (collision.gameObject.CompareTag("Player"));
        {
            if (collision.gameObject != owner)
            {
                Bounce(1);
            }
        }
        
        
    }

    private void ApplyForce()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(horizontalDirection * horizontalSpeed, verticalDirection * verticalSpeed);
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
    
    public void SetHorizontalDirection(int direction)
    {
        horizontalDirection = direction;
    }
    
    public void SetVerticalDirection(int direction)
    {
        verticalDirection = direction;
    }
    
    public void SetHorizontalSpeed(float speed)
    {
        horizontalSpeed = speed;
    }
    
    public void SetVerticalSpeed(float speed)
    {
        verticalSpeed = speed;
    }
    
    public int GetHorizontalDirection()
    {
        return horizontalDirection;
    }
    
    public int GetVerticalDirection()
    {
        return verticalDirection;
    }
    
}
