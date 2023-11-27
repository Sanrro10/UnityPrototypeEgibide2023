using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class LandWitch : EntityControler
{
    [SerializeField] private GameObject player;

    public LandWitchData landWitchData;

    private bool _isActive = false;
    private bool _canLaunchMissile = false;
    private bool _isLaunchingMissiles = false;
    private bool _canMagicCircle = false;
    private bool _isLaunchingMagicCircles = false;
    
    private GameObject _playerRef;
    private SpriteRenderer _spriteWitch;
    
    public GameObject witchMissile;
    public GameObject witchMagicCircle;

    
    /*Ideas Witch, make so that if the player stays longer than x time near the witch, she TPs*/
    
    // Start is called before the first frame update
    void Start()
    {
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", landWitchData.health, SendMessageOptions.RequireReceiver);
        _playerRef = GameController.Instance.GetPlayerGameObject();
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
    public void SetActiveState(bool state)
    {
        //Invoke or Uninvoke attack patterns
        //Turn towards player
        _isActive = state;
        if (_isActive)
        {
            InvokeRepeating(nameof(TurnToPlayer) , 1 , 1);
            InvokeRepeating(nameof(WitchAttack),0,0.5f);
            Invoke(nameof(WitchMainTeleport), landWitchData.normalTeleportationCooldown);
        }
        else
        {
            CancelInvoke(nameof(TurnToPlayer));
            CancelInvoke(nameof(WitchAttack));
            CancelInvoke(nameof(WitchMainTeleport));
        }
    }
    /*Tells the LandWitch that she can perform her missile attack(or not)*/
    public void SetMissilePossible(bool state)
    {
        _canLaunchMissile = state;
    }
    /*Tells the LandWitch that she can perform her magic circle attack(or not)*/
    public void SetMagicCirclePossible(bool state)
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
            CancelInvoke(nameof(LaunchEvilMissile));
        }

        if (!_canMagicCircle && !_isLaunchingMagicCircles)
        {
            CancelInvoke(nameof(LaunchMagicCircle));
        }


    }

    private void AccionateMissileLogic()
    {
        _isLaunchingMissiles = true;
        _isLaunchingMagicCircles = false;
        CancelInvoke(nameof(LaunchMagicCircle));
        StopCoroutine(nameof(WitchFastTeleport));
        InvokeRepeating(nameof(LaunchEvilMissile),0, landWitchData.missileCooldown);
    }

    private void AccionateMagicCircleLogic()
    {
        _isLaunchingMagicCircles = true;
        _isLaunchingMissiles = false;
        CancelInvoke(nameof(LaunchEvilMissile));
        StopCoroutine(nameof(WitchFastTeleport));
        InvokeRepeating(nameof(LaunchMagicCircle) , 0, landWitchData.magicCircleCooldown);
            
    }

    private void AccionateFastTeleportLogic()
    {
        _isLaunchingMissiles = false;
        _isLaunchingMagicCircles = false;
        StopCoroutine(nameof(WitchFastTeleport));
        StartCoroutine(nameof(WitchFastTeleport),0);
    }

    /*LandWitch Main Teleport, continously working*/
    private void WitchMainTeleport()
    {
        CheckForTeleportPlaces();
        Invoke(nameof(WitchMainTeleport), landWitchData.normalTeleportationCooldown);
    }
    
    /*LandWitch Fast Teleport, cancels temporarily main teleport*/
    private void WitchFastTeleport()
    {
        //TODO:
        //throw new NotImplementedException();

        yield return new WaitForSeconds(landWitchData.fastTeleportationCooldown);
    }

    private void LaunchEvilMissile()
    {

            Debug.Log("Missile Launch");
            
            Instantiate(witchMissile, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2), Quaternion.identity);
            //Invoke(nameof(PotionCooldown),1);
            
       
    }

    private void LaunchMagicCircle()
    {
        //TODO:
        Debug.Log("Magic Circle Launch");

        Instantiate(witchMagicCircle, new Vector2(_playerRef.transform.position.x, _playerRef.transform.position.y), Quaternion.identity);
        //throw new NotImplementedException();
    }


    private void CheckForTeleportPlaces()
    {
        var plusMinus = RandomSign();
        var checkingPosition = GetCheckingPositionPositionCoords(plusMinus);
        
        var leftLimit = gameObject.transform.position.x - 5;
        var rightLimit = gameObject.transform.position.y + 5;

        byte limitsChecked = 0;
        
        bool placeFound = false;
        RaycastHit2D possiblePos;
        
        while (!placeFound)
        {
            /*If no position is returned after checking both sides, it will teleport to itself*/
            if (limitsChecked == 2)
            {
                placeFound = true;
                possiblePos = Physics2D.Raycast(gameObject.transform.position, Vector2.down);
            }
            else
            {
                /*A ray is thrown downwards to strike all of the wrongdoers of the fantasy world of Akulapakua*/
                possiblePos = Physics2D.Raycast(checkingPosition, Vector2.down);
                if (possiblePos.collider != null && !possiblePos.collider.gameObject.CompareTag("Player"))
                {
                    placeFound = true;
                }
                else
                {
                    if (checkingPosition.x == leftLimit || checkingPosition.x == rightLimit)
                    {
                        limitsChecked++;
                        plusMinus *= -1;
                        checkingPosition = GetCheckingPositionPositionCoords((plusMinus));
                    }
                    else
                    {
                        checkingPosition.x = checkingPosition.x + (3 * plusMinus);
                    }
                }
            }
        }

        Vector2 posToTeleport = new Vector2(possiblePos.point.x, possiblePos.point.y);
        
        Teleport(posToTeleport);

    }
    /*Returns 1 or -1*/
    public int RandomSign()
    {
        return (Random.value > 0.5f ? 1 : -1);
    }
    
    /*Returns the position to check in order to teleport*/
    private Vector2 GetCheckingPositionPositionCoords(int sign)
    {
        var startingPositionX = gameObject.transform.position.x +(20 * sign);
        var startingPositionY = gameObject.transform.position.y + 10;

        return new Vector2(startingPositionX, startingPositionY);
    }

    /*Teleports the witch*/
    private void Teleport(Vector2 posToTeleport)
    {
        gameObject.transform.position = posToTeleport;
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
