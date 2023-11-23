using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EvilMissile : MonoBehaviour
{
    
    private GameObject _originWitch;
    [FormerlySerializedAs("_speed")] public float speed;

 //   [SerializeField] private int health;
 
    [SerializeField] private GameObject explosion;

    private Rigidbody2D _missileBody;
    private GameObject _playerRef;

    private float _angle;
    void Start()
    {
        //_player = GameObject.Find("Player Espada State");
        //_playerController = _player.GetComponent<PlayerController>();
        
        _playerRef = GameObject.Find("Player Espada State");
        
        _missileBody = gameObject.GetComponent<Rigidbody2D>();
        
        Rotacion();
        StartCoroutine("ApplyForce",0f);
        Invoke("MaximumAliveTime",15f);
        
        
    }

    /*Rotates the proyectile so that its X rotation var points to the player*/
    private void Rotacion()
    {
        var neededRotation = Quaternion.LookRotation(_playerRef.transform.position - transform.position);
        transform.rotation = neededRotation;
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

        Destroy(gameObject);

    }

    /*Launches the Missile Towards the player*/
    private IEnumerator ApplyForce()
    {
        Debug.Log("UOOOOOOOOOOOOOOO NEVADOOOOOOOOOOOOOOOOOOOO");
        yield return new WaitForSeconds(0.5f);
        
        Vector2 whereToGoPlease = _playerRef.transform.position - transform.position;
        whereToGoPlease.Normalize();
        Vector2 speedwagon = whereToGoPlease * speed;
        _missileBody.velocity = speedwagon;
        //gameObject.GetComponent<Rigidbody2D>().AddForce(transform.forward * _speed * 2f);
    }
    

    private void Explode()
    {   
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void MaximumAliveTime()
    {
        Destroy(gameObject);
    }

}
