using System;
using General.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities.Enemies.Witch.Scripts
{
    public class LandWitch : EntityControler
    {
        private GameObject _player;

        public LandWitchData landWitchData;

        [SerializeField] private Animator witchAnimator;
        
        private bool _isActive = false;
        private bool _canLaunchMissile = false;
        private bool _isLaunchingMissiles = false;
        private bool _canMagicCircle = false;
        private bool _isLaunchingMagicCircles = false;
        private bool _hasBeenActivated = false;
        private int _lastCheckedHealth;
        
        private GameObject[] _puntosTeleport;
        private int _lastTeleportPoint;
    
        private GameObject _playerRef;
        [SerializeField] private SpriteRenderer spriteWitch;
    
        public GameObject witchMissile;
        public GameObject witchMagicCircle;
    
        // Start is called before the first frame update
        void Start()
        {
            
            //Set the Health Points
            gameObject.GetComponent<HealthComponent>().SendMessage("Set", landWitchData.health, SendMessageOptions.RequireReceiver);
            _lastCheckedHealth = landWitchData.health;
            _playerRef = GameController.Instance.GetPlayerGameObject();
            _puntosTeleport = GameObject.FindGameObjectsWithTag("WitchTeleport");
        }

        /*Activation/Deactivation of the LandWitch, it starts the function needed to attack
     and starts tracking the player which means, turning towards it[Called Externally]*/
        public void SetActiveState(bool state)
        {
            //Invoke or Cancel attack patterns
            //Turn towards player
            _isActive = state;
            if (_isActive)
            {
                InvokeRepeating(nameof(TurnToPlayer) , 1 , 1);
                InvokeRepeating(nameof(WitchAttack),0,0.5f);
                if (!_hasBeenActivated)
                {
                    _hasBeenActivated = true;
                    Invoke(nameof(WitchMainTeleport), landWitchData.normalTeleportationCooldown);    
                }
            }
            else
            {
                CancelInvoke(nameof(TurnToPlayer));
                CancelInvoke(nameof(WitchAttack));
            }
        }
        /*Tells the LandWitch that she can perform her missile attack(or not)[Called Externally]*/
        public void SetMissilePossible(bool state)
        {
            _canLaunchMissile = state;
        }
        /*Tells the LandWitch that she can perform her magic circle attack(or not)[Called Externally]*/
        public void SetMagicCirclePossible(bool state)
        {
            _canMagicCircle = state;
        }


        /*The LandWitch tries to face the players position*/
        private void TurnToPlayer()
        {

            var playerRelativePos = gameObject.transform.InverseTransformPoint(_playerRef.transform.position).x;
            
            if (playerRelativePos > 0)
            {
                Debug.Log("Derecha");
                spriteWitch.flipX = true;
            }else if (playerRelativePos < 0)
            {
                Debug.Log("Izquierda");
                spriteWitch.flipX = false;
            }

        }

        /*Handler of the logic of the LandWitch's attack patterns*/
        private void WitchAttack()
        {
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
            if (!_canLaunchMissile && !_canMagicCircle)
            {
                AccionateFastTeleportLogic();
            }
            //Cancel activation of missile if can't launch it
            if (!_canLaunchMissile && !_isLaunchingMissiles)
            {
                CancelInvoke(nameof(LaunchEvilMissile));
                CancelInvoke(nameof(ActivateAnimMissile));
            }
            //Cancel activation of MagicCircle if can't launch it
            if (!_canMagicCircle && !_isLaunchingMagicCircles)
            {
                CancelInvoke(nameof(LaunchMagicCircle));
                CancelInvoke(nameof(ActivateAnimMagicCircle));
            }


        }

        /*Manages the logic and variables needed to launch missiles and cancels other attacks*/
        private void AccionateMissileLogic()
        {
            _isLaunchingMissiles = true;
            _isLaunchingMagicCircles = false;
            CancelInvoke(nameof(LaunchMagicCircle));
            CancelInvoke(nameof(ActivateAnimMagicCircle));
            InvokeRepeating(nameof(ActivateAnimMissile), 0, landWitchData.missileCooldown);
            InvokeRepeating(nameof(LaunchEvilMissile),0.5f, landWitchData.missileCooldown);
        }

        /*Manages the logic and variables needed to launch MagicCircles and cancels other attacks*/
        private void AccionateMagicCircleLogic()
        {
            _isLaunchingMagicCircles = true;
            _isLaunchingMissiles = false;
            CancelInvoke(nameof(LaunchEvilMissile));
            CancelInvoke(nameof(ActivateAnimMissile));
            InvokeRepeating(nameof(ActivateAnimMagicCircle),0,landWitchData.magicCircleCooldown);
            InvokeRepeating(nameof(LaunchMagicCircle) , 0.2f, landWitchData.magicCircleCooldown);
            
        }

        /*Teleports the witch on receiveing damage*/
        private void AccionateDamageLogic()
        {
            CancelInvoke(nameof(WitchFastTeleport));
            CancelInvoke(nameof(ActivateWitchDeathDamageFast));
            Invoke(nameof(ActivateWitchDeathDamageFast), 0);
            Invoke(nameof(WitchFastTeleport),0.5f);
        }

        /*Invokes the witch's fast teleport*/
        private void AccionateFastTeleportLogic()
        {
            CancelInvoke(nameof(WitchFastTeleport));
            CancelInvoke(nameof(ActivateWitchDeathDamageFast));
            Invoke(nameof(ActivateWitchDeathDamageFast), landWitchData.fastTeleportationCooldown - 0.5f);
            Invoke(nameof(WitchFastTeleport),landWitchData.fastTeleportationCooldown);
        }

        /*Executes the Witch's Main Teleport, continuously working*/
        private void WitchMainTeleport()
        {
            CancelInvoke(nameof(LaunchEvilMissile));
            CancelInvoke(nameof(ActivateAnimMissile));
            CancelInvoke(nameof(LaunchMagicCircle));
            CancelInvoke(nameof(ActivateAnimMagicCircle));
            CheckForTeleportPoints();
            Invoke(nameof(ActivateAnimTeleport), landWitchData.normalTeleportationCooldown - 0.5f);
            Invoke(nameof(WitchMainTeleport), landWitchData.normalTeleportationCooldown);
        }
    
        /*Executes Fast Teleport, cancels temporarily main teleport*/
        private void WitchFastTeleport()
        {
            CancelInvoke(nameof(WitchMainTeleport));
            CancelInvoke(nameof(ActivateAnimTeleport));
            CheckForTeleportPoints();
            Invoke(nameof(ActivateAnimTeleport), landWitchData.normalTeleportationCooldown - 0.5f);
            Invoke(nameof(WitchMainTeleport), landWitchData.normalTeleportationCooldown);
        }
    
        /*Instantiates a new Missile and Activates Animation of Missile attack*/
        private void LaunchEvilMissile()
        {
            Instantiate(witchMissile, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 2), Quaternion.identity);
        }

        /*Instantiates a new MagicCircle and Activates Animation of Magic Circle*/
        private void LaunchMagicCircle()
        {
            Instantiate(witchMagicCircle, new Vector2(_playerRef.transform.position.x, _playerRef.transform.position.y), Quaternion.identity);
        }

        /*Checks for prepositioned teleport points, and moves the witch to one of them at random,
     avoiding the last one that was moved to, and activates teleport animation*/
        private void CheckForTeleportPoints()
        {
            var numberOfTeleportPoint = _puntosTeleport.Length;
            GameObject newTeleportPoint = new GameObject();
            bool puntoEncontrado = false;
        
            while (!puntoEncontrado)
            {
                var newRandom = (int)(Random.Range(0f, numberOfTeleportPoint));
                newTeleportPoint = _puntosTeleport[newRandom];
                if (newRandom != _lastTeleportPoint)
                {
                    _lastTeleportPoint = newRandom;
                    puntoEncontrado = true;
                    gameObject.transform.position = newTeleportPoint.transform.position;
                }
            }


           

        }

        /*ANIMATION TRIGGER ACTIVATORS*/

        private void ActivateAnimMissile()
        {
            witchAnimator.SetTrigger("WitchAttackTrigger");
        }

        private void ActivateAnimMagicCircle()
        {
            witchAnimator.SetTrigger("WitchCircleTrigger");
        }

        private void ActivateAnimTeleport()
        {
            witchAnimator.SetTrigger("WitchTeleport");
        }

        private void ActivateWitchDeathDamageFast()
        {
            witchAnimator.SetTrigger("WitchDeathDamage");
        }

        /*Launches the teleport animation, then Destroys thw witch*/
        public override void OnDeath()
        {
            Invoke(nameof(ActivateAnimTeleport),0);
            Invoke(nameof(Delete),0.5f);
        }

        public override void OnReceiveDamage(int damage)
        {
            base.OnReceiveDamage(damage);
            Invoke(nameof(AccionateDamageLogic),0);
        }

        /*Calls the delete function on this gameObject*/
        private void Delete()
        {
            Destroy(gameObject);
        }
        /*NOT IN USE //  POSSIBLE CASE OF USE SO NO DELETE*/
        /*private void CheckForTeleportPlaces()
    {
        

        var plusMinus = RandomSign();
        var checkingPosition = GetCheckingPositionCoords(plusMinus);
        
        var leftLimit = gameObject.transform.position.x - 5;
        var rightLimit = gameObject.transform.position.y + 5;

        byte limitsChecked = 0;
        
        bool placeFound = false;
        RaycastHit2D possiblePos  = Physics2D.Raycast(checkingPosition, Vector2.down);
        
        while (!placeFound)
        {
            /*If no position is returned after checking both sides, it will teleport to itself#1#
            if (limitsChecked == 2)
            {
                placeFound = true;
                possiblePos = Physics2D.Raycast(gameObject.transform.position, Vector2.down);
            }
            else
            {
                /*A ray is thrown downwards to strike all of the wrongdoers of the fantasy world of Akulapakua
                 Or to check if a teleport is possible#1#
                possiblePos = Physics2D.Raycast(checkingPosition, Vector2.down);
                var checkForRoom = Physics2D.Raycast(possiblePos.point, Vector2.up);
                var spaceToTP = checkForRoom.point.y - possiblePos.point.y;
                if (possiblePos.collider != null && !possiblePos.collider.gameObject.CompareTag("Player")
                                                 && possiblePos.collider.gameObject.CompareTag("Floor")
                                                 && spaceToTP >= 3)
                {
                    placeFound = true;
                }
                else
                {
                    if ((plusMinus == -1 && checkingPosition.x > leftLimit) || 
                        (plusMinus == 1 && checkingPosition.x < rightLimit))
                    {
                        limitsChecked++;
                        plusMinus *= -1;
                        checkingPosition = GetCheckingPositionCoords((plusMinus));
                    }
                    else
                    {
                        Debug.DrawLine(checkingPosition, possiblePos.point, Color.magenta, 0.5f);
                        checkingPosition.x += (3 * -plusMinus);
                    }
                }
            }
        }
        
        Vector2 posToTeleport = new Vector2(possiblePos.point.x, possiblePos.point.y);
        
        Teleport(posToTeleport);

    }
    /*Returns 1 or -1#1#
    public int RandomSign()
    {
        return (Random.value > 0.5f ? 1 : -1);
    }
    
    /*Returns the position to check in order to teleport#1#
    private Vector2 GetCheckingPositionCoords(int sign)
    {
        var startingPositionX = gameObject.transform.position.x +(20 * sign);
        var startingPositionY = gameObject.transform.position.y + 10;

        return new Vector2(startingPositionX, startingPositionY);
    }

    /*Teleports the witch#1#
    private void Teleport(Vector2 posToTeleport)
    {
        gameObject.transform.position = posToTeleport;
    }*/
    
    }
}
