using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandWitch : EntityControler
{
    [SerializeField] private GameObject player;

    [SerializeField] private LandWitchData landWitchData;
    private GameObject _playerRef;
    private bool _throwingPotions = false;
    
    private float _angle;
    
    private bool _onPotionColdown;
    public GameObject evilPotion;
    private object _sliderPotion;
    
    /*Ideas Witch, make so that if the player stays longer than x time near the witch, she TPs*/
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", landWitchData.health, SendMessageOptions.RequireReceiver);
        _playerRef = GameObject.Find("Player Espada State");
        
        _angle = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {   
        Debug.Log("Ha entrado algo en mi radio de accion JIJIJI");
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>() != null)
        {
            Debug.Log("Cuidado, es el heroe");
            //Start timer to throw potions at the player, and, a very much longer timer to Teleport
            InvokeRepeating("WitchAttack", 1,3);
            _throwingPotions = true;
            //Start timer to Teleport the witch/ Or maybe only whenever she cant attack the player
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>() != null)
        {
            //End the timer that throws potions, but the tp one, once activated, will follow the player periodically
            CancelInvoke("WitchAttack");
        }
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

    private void WitchAttack()
    {
        //JIJIJIJIJI-audio laugh
        getAngle();
        _onPotionColdown = false;
        EvilPotion();
        
    }

    private void EvilPotion()
    {
        if (!_onPotionColdown)
        {
            Debug.Log("POTION LAUNCH");
            
            Instantiate(evilPotion, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2), Quaternion.identity);
            _onPotionColdown = true;
            //Invoke(nameof(PotionCooldown),1);
            
        }
    }
        
        
    /*public void PotionCooldown()
    {
        _onPotionColdown = false;
    }*/
    
}
