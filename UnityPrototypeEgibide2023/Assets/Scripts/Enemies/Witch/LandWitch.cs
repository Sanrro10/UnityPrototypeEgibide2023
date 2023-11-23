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
    private bool _isLaunchingMissiles = false;
    private bool _canMagicCircle = false;
    private bool _isLaunchingMagicCircles = false;
    private GameObject _playerRef;
    private SpriteRenderer _spriteWitch;
    
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
        _spriteWitch = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }
    /*Activation/Deactivation of the LandWitch, it starts the function needed to attack
     and starts tracking the player which means, turning towards it*/
    public void setActiveState(bool state)
    {
        //Invoke or Uninvoke attack patterns
        //Turn towards player
        _isActive = state;
        Debug.Log("Se ha entrado aqui");
        if (_isActive)
        {
            InvokeRepeating("TurnToPlayer" , 1 , 1);
            InvokeRepeating("WitchAttack",0,0.5f);
            StartCoroutine("WitchMainTeleport", landWitchData.normalTeleportationCooldown);
        }
        else
        {
            CancelInvoke("TurnToPlayer");
            CancelInvoke("WitchAttack");
            StopCoroutine("WitchMainTeleport");
        }
    }
    /*Tells the LandWitch that she can perform her missile attack(or not)*/
    public void setMissilePossible(bool state)
    {
        _canLaunchMissile = state;
    }
    /*Tells the LandWitch that she can perform her magic circle attack(or not)*/
    public void setMagicCirclePossible(bool state)
    {
        _canMagicCircle = state;
    }


    /*The LandWitch tries to face the players position*/
    private void TurnToPlayer()
    {
        if (gameObject.transform.InverseTransformPoint(_playerRef.transform.position).x > 0)
        {
            _spriteWitch.flipX = true;
        }else if (gameObject.transform.InverseTransformPoint(_playerRef.transform.position).x < 0)
        {
            _spriteWitch.flipX = false;
        }

    }

    /*Handler of the logic of the LandWitch's attack patterns*/
    private void WitchAttack()
    {
        //JIJIJIJIJI-audio laugh
        
        //Launch Missiles
        if (_canLaunchMissile && !_isLaunchingMissiles && (!_canMagicCircle))
        {
            AccionateMissileLogic();   
        }
        
        //Launch Magic Circles - When doing MC, the witch can missile, so no check on that
        if (_canMagicCircle && !_isLaunchingMagicCircles)
        {
            AccionateMagicCircleLogic();
        }
        
        //Activate Fast teleport
        if (_isActive && !_canLaunchMissile && !_canMagicCircle)
        {
            AccionateFastTeleportLogic();
        }

        if (!_canLaunchMissile && !_isLaunchingMissiles)
        {
            CancelInvoke("LaunchEvilMissile");
        }

        if (!_canMagicCircle && !_isLaunchingMagicCircles)
        {
            CancelInvoke("LaunchMagicCirle");
        }


    }

    private void AccionateMissileLogic()
    {
        _isLaunchingMissiles = true;
        _isLaunchingMagicCircles = false;
        CancelInvoke("LaunchMagicCirle");
        StopCoroutine("WitchFastTeleport");
        InvokeRepeating("LaunchEvilMissile",0, landWitchData.missileCooldown);
    }

    private void AccionateMagicCircleLogic()
    {
        _isLaunchingMagicCircles = true;
        _isLaunchingMissiles = false;
        CancelInvoke("LaunchEvilMissile");
        StopCoroutine("WitchFastTeleport");
        InvokeRepeating("LaunchMagicCirle" , 0, landWitchData.magicCircleCooldown);
            
    }

    private void AccionateFastTeleportLogic()
    {
        _isLaunchingMissiles = false;
        _isLaunchingMagicCircles = false;
        StopCoroutine("WitchFastTeleport");
        StartCoroutine("WitchFastTeleport",0);
    }

    /*LandWitch Main Teleport, continously working*/
    private IEnumerator WitchMainTeleport()
    {
        //TODO:
        //throw new NotImplementedException();

        yield return new WaitForSeconds(landWitchData.normalTeleportationCooldown);
    }
    
    /*LandWitch Fast Teleport, cancels temporarily main teleport*/
    private IEnumerator WitchFastTeleport()
    {
        //TODO:
        //throw new NotImplementedException();

        yield return new WaitForSeconds(landWitchData.fastTeleportationCooldown);
    }

    private void LaunchEvilMissile()
    {

            Debug.Log("Missile Lanuch");
            
            Instantiate(witchMissile, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2), Quaternion.identity);
            //Invoke(nameof(PotionCooldown),1);
            
       
    }

    private void LaunchMagicCirle()
    {
        //TODO:
        //throw new NotImplementedException();
    }

    
    /*Example timer
     
             public IEnumerator GroundedCooldown()
        {
                feetBoxCollider.enabled = false;
                yield return new WaitForSeconds(0.2f);
                feetBoxCollider.enabled = true;
        }
        
    */
}
