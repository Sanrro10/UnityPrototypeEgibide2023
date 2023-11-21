using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandWitch : EntityControler
{
    [SerializeField] private GameObject player;

    [SerializeField] private LandWitchData landWitchData;

    private bool _isActive = false;
    private bool _canLaunchMissile = false;
    private bool _canMagicCircle = false;
    private GameObject _playerRef;
    
    //consider deleting this var
    private bool _throwingPotions = false;
    
    
    public GameObject witchMissile;

    
    /*Ideas Witch, make so that if the player stays longer than x time near the witch, she TPs*/
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", landWitchData.health, SendMessageOptions.RequireReceiver);
        _playerRef = GameObject.Find("Player Espada State");
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            
        }
    }

    public void setActiveState(bool state)
    {
        //Invoke or Uninvoke attack patterns
        //Turn towards player
        _isActive = state;
        Debug.Log("Se ha entrado aqui");
        if (_isActive)
        {
            InvokeRepeating("TurnToPlayer" , 1 , 1);
        }
        else
        {
            CancelInvoke("TurnToPlayer");
        }
    }
    
    public void setMissilePossible(bool state)
    {
        _canLaunchMissile = state;
    }

    public void setMagicCirlePossible(bool state)
    {
        _canMagicCircle = state;
    }


    private void TurnToPlayer()
    {
        if (gameObject.transform.InverseTransformPoint(_playerRef.transform.position).x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }else if (gameObject.transform.InverseTransformPoint(_playerRef.transform.position).x < 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    private void WitchAttack()
    {
        //JIJIJIJIJI-audio laugh

        EvilMissile();
        
    }

    private void EvilMissile()
    {
        if (_canLaunchMissile)
        {
            Debug.Log("POTION LAUNCH");
            
            Instantiate(witchMissile, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2), Quaternion.identity);
            //Invoke(nameof(PotionCooldown),1);
            
        }
    }
    
        
    /*public void PotionCooldown()
    {
        _onPotionColdown = false;
    }*/
    
}
