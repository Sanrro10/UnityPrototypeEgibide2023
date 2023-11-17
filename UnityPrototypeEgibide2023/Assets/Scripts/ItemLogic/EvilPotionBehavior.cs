using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvilPotionBehavior : MonoBehaviour
{
    
    private GameObject _originWitch;



    [SerializeField] private float _speed;

 //   [SerializeField] private int health;
    
    [SerializeField] private GameObject owner;
    [SerializeField] private GameObject explosion;

    private GameObject _playerRef;

    private float _angle;
    void Start()
    {
        //_player = GameObject.Find("Player Espada State");
        //_playerController = _player.GetComponent<PlayerController>();
        
        
        ApplyForce();
        
    }

    private void getAngle()
    {
        //here the degrees between the witch and the player are calculated, having the witch as the center of the circle
        var myPos = transform.position;
        
        var targetPos = _playerRef.transform.position;

        Vector2 toOther = (myPos - targetPos).normalized;
    
        /*Here is where the math... ejem, magic happens.
            By giving the parameter y, the value of x ; and the parameter x, the value of y,
            we obtain an angle ANDDDD have the 0º be up in the sky, then, because the angles we want
            are either to the left or to the right, we can pick them up easily
        */
        _angle = Mathf.Atan2(toOther.x, toOther.y) * Mathf.Rad2Deg + 180;
        
        
        
        //this makes the angles be only between 0 and 180, negatives included
         
        _angle = _angle % 360;
        _angle = (_angle + 360) % 360;
        if (_angle > 180)
        {
            _angle -= 360;
        }
        Debug.Log(_angle);
        Debug.DrawLine(myPos, targetPos, Color.magenta);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        /*// if collision is against floor 
        else if (collision.gameObject.CompareTag("Player"))
        {
            Explode();
        } 
        else
        {
            Bounce(1);
        }*/

        Explode();

    }

    private void ApplyForce()
    {
            GetComponent<Rigidbody2D>().velocity = new Vector2(_playerRef.gameObject.transform.position.x, _playerRef.gameObject.transform.position.y);
    }

    
    /*private void Bounce(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Explode();
        }
    }*/
    

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
