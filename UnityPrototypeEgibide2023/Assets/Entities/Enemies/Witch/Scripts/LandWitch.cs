using System;
using General.Scripts;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Entities.Enemies.Witch.Scripts
{
    public class LandWitch : EntityControler
    {
        public LandWitchData landWitchData;

        [SerializeField] private Animator witchAnimator;
        private AnimationClip _clip;

        private float _missileTime;
        private float _circleTime;
        private float _laughtTime;
        private float _tpTime;
        private float _angerLevel = 1;
        
        private bool _isActive = false;
        private bool _canLaunchMissile = false;
        private bool _isLaunchingMissiles = false;
        private bool _canMagicCircle = false;
        private bool _isLaunchingMagicCircles = false;
        private bool _hasBeenActivated = false;
        private int _lastCheckedHealth;

        private Material materialBruja;
        
        [SerializeField] private GameObject[] puntosTeleport;
        private int _lastTeleportPoint;
        private int _teleportPointClosestToPlayer;
    
        private GameObject _playerRef;
        [SerializeField] private SpriteRenderer spriteWitch;
    
        public GameObject witchMissile;
        public GameObject witchMagicCircle;

        public static event Action AllyDeath;
    
        // Start is called before the first frame update
        void Start()
        {

            materialBruja = gameObject.GetComponentInChildren<SpriteRenderer>().material;
            materialBruja.SetFloat("_HitEffectGlow", 5f);
            materialBruja.SetFloat("_HitEffectBlend", 0.2f);
            //Set the Health Points
            gameObject.GetComponent<HealthComponent>().SendMessage("Set", landWitchData.health, SendMessageOptions.RequireReceiver);
            _lastCheckedHealth = landWitchData.health;
            _playerRef = GameController.Instance.GetPlayerGameObject();
            if (puntosTeleport.Length == 0 || puntosTeleport == null)
            {
                puntosTeleport = GameObject.FindGameObjectsWithTag("WitchTeleport");
            }

            AnimationLength();
            AllyDeath += SelfDeath;
        }

        private void SelfDeath()
        {
            _angerLevel = _angerLevel + landWitchData.angerIncrement;
        }

        private void OnDestroy()
        {
            AllyDeath -= SelfDeath;
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
                InvokeRepeating(nameof(TurnToPlayer) , 0.03f , 0.03f);
                InvokeRepeating(nameof(WitchAttack),0,0.5f);
                if (!_hasBeenActivated)
                {
                    _hasBeenActivated = true;
                    Invoke(nameof(ActivateAnimTeleport), landWitchData.normalTeleportationCooldown);    
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
                FacingRight = false;
            }else if (playerRelativePos < 0)
            {
                FacingRight = true;
            }
            
            int objective = FacingRight ? 0:180;
            if ((int)spriteWitch.gameObject.transform.rotation.eulerAngles.y !=  objective)
            {
                spriteWitch.gameObject.transform.eulerAngles = new UnityEngine.Vector3(spriteWitch.transform.transform.eulerAngles.x, spriteWitch.transform.rotation.eulerAngles.y + (FacingRight ? -30: 30), spriteWitch.transform.rotation.eulerAngles.z);

            }

        }

        /*Handler of the logic of the LandWitch's attack patterns*/
        private void WitchAttack()
        {
            //Launch Missiles
            if (/*_canLaunchMissile &&*/ !_isLaunchingMissiles && (!_canMagicCircle))
            {
                AccionateMissileLogic();
                return;
            }
        
            //Launch Magic Circles - When doing MC, the witch can missile, so no check on that
            if (_canMagicCircle && !_isLaunchingMagicCircles)
            {
                AccionateMagicCircleLogic();
                return;
            }
        
            //AttackWhenNot in range
            // if (!_canLaunchMissile && !_canMagicCircle && !_isLaunchingMissiles)
            // {
            //     //AccionateFastTeleportLogic();
            //     AccionateMissileLogic();
            // }
            //Cancel activation of missile if can't launch it
            if (!_canLaunchMissile && !_isLaunchingMissiles)
            {
                CancelInvoke(nameof(LaunchEvilMissile));
                CancelInvoke(nameof(ActivateAnimMissile));
                return;
            }
            //Cancel activation of MagicCircle if can't launch it
            if (!_canMagicCircle && !_isLaunchingMagicCircles)
            {
                CancelInvoke(nameof(LaunchMagicCircle));
                CancelInvoke(nameof(ActivateAnimMagicCircle));
                return;
            }


        }

        /*Manages the logic and variables needed to launch missiles and cancels other attacks*/
        private void AccionateMissileLogic()
        {
            _isLaunchingMissiles = true;
            _isLaunchingMagicCircles = false;
            CancelInvoke(nameof(LaunchMagicCircle));
            CancelInvoke(nameof(ActivateAnimMagicCircle));
            InvokeRepeating(nameof(ActivateAnimMissile), 0, landWitchData.missileCooldown / _angerLevel);
            InvokeRepeating(nameof(LaunchEvilMissile),0.5f, landWitchData.missileCooldown / _angerLevel);
            Debug.Log($"{landWitchData.missileCooldown / _angerLevel}");
        }

        /*Manages the logic and variables needed to launch MagicCircles and cancels other attacks*/
        private void AccionateMagicCircleLogic()
        {
            _isLaunchingMagicCircles = true;
            _isLaunchingMissiles = false;
            CancelInvoke(nameof(LaunchEvilMissile));
            CancelInvoke(nameof(ActivateAnimMissile));
            InvokeRepeating(nameof(ActivateAnimMagicCircle),0,landWitchData.magicCircleCooldown / _angerLevel);
            InvokeRepeating(nameof(LaunchMagicCircle) , 0.2f, landWitchData.magicCircleCooldown / _angerLevel);
            
        }

        /*Teleports the witch on receiveing damage*/
        private void AccionateDamageLogic()
        {
            CancelAllInvokes();
            ActivateAnimTeleport();
        }

        /*Executes the Witch's Main Teleport, continuously working*/
        // private void WitchMainTeleport()
        // {
        //     CancelInvoke(nameof(LaunchEvilMissile));
        //     CancelInvoke(nameof(ActivateAnimMissile));
        //     CancelInvoke(nameof(LaunchMagicCircle));
        //     CancelInvoke(nameof(ActivateAnimMagicCircle));
        //     Invoke(nameof(ActivateAnimTeleport), landWitchData.normalTeleportationCooldown);
        // }
    
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
            var numberOfTeleportPoint = puntosTeleport.Length;
            GameObject newTeleportPoint;
            bool puntoEncontrado = false;
        
            while (!puntoEncontrado)
            {
                var newRandom = (int)(Random.Range(0f, numberOfTeleportPoint));
                newTeleportPoint = puntosTeleport[newRandom];
                if (newRandom != _lastTeleportPoint)
                {
                    _lastTeleportPoint = newRandom;
                    puntoEncontrado = true;
                    gameObject.transform.position = newTeleportPoint.transform.position;
                }
            }
        }
        
        /*Checks the closest point to the player, to prevent the witch from moving to it, and trying to maintain distance
         to player;*/
        private void CheckPointClosestToPlayer()
        {
            _teleportPointClosestToPlayer = -1;
            float distancex = -1;
            float distancey = -1;
            foreach (GameObject punto in puntosTeleport)
            {
                distancex = punto.transform.position.x;
                distancey = punto.transform.position.y;


            }   
        }

        /*ANIMATION BOOLEAN ACTIVATION AND DEACTIVATION*/
        
        /*Activates the Missile Animation*/
        private void ActivateAnimMissile()
        {
            witchAnimator.SetBool("WitchMissile", true);
            Invoke(nameof(DeactivateAnimMissile), _missileTime);
        }

        /*Deactivates the Missile Animation*/
        private void DeactivateAnimMissile()
        {
            witchAnimator.SetBool("WitchMissile", false);
        }

        
        /*Activates the Magic Circle Animation*/
        private void ActivateAnimMagicCircle()
        {
            witchAnimator.SetBool("WitchCircle", true);
            Invoke(nameof(DeactivateAnimMagicCircle), _circleTime);
        }

        
        /*Deactivates the Magic Circle Animation*/
        private void DeactivateAnimMagicCircle()
        {
            witchAnimator.SetBool("WitchCircle", false);
        }

        
        /*Deactivates ALL other animations and Activates the Teleport Animation*/
        private void ActivateAnimTeleport()
        {
            DeactivateAnimMissile();
            DeactivateAnimMagicCircle();
            witchAnimator.SetBool("WitchTeleport", true);
            Invoke(nameof(DeactivateAnimTeleport) , _tpTime);
        }

        /*Deactivates the teleport animation and teleports the witch*/
        private void DeactivateAnimTeleport()
        {
            witchAnimator.SetBool("WitchTeleport", false);
            CheckForTeleportPoints();
            Invoke(nameof(ActivateAnimTeleport),landWitchData.normalTeleportationCooldown);
            //Invoke(nameof(WitchMainTeleport), 0);
        }

        /*Activates the Death teleport animation*/
        private void ActivateWitchDeathFast()
        {
            witchAnimator.SetBool("WitchDeathDmg", true);
        }


        private void AnimationLength()
        {
            AnimationClip[] clips = witchAnimator.runtimeAnimatorController.animationClips;
            foreach (AnimationClip clip in clips)
            {
                switch (clip.name)
                {
                    case "WitchTeleport":
                        _tpTime = clip.length;
                        break;
                    case "WitchMissileAttack":
                        _missileTime = clip.length;
                        break;
                    case "WitchMagicCircle":
                        _circleTime = clip.length;
                        break;
                    case "WitchLaught":
                        _laughtTime = clip.length;
                        break;
                }
            }
        }

        /*Convenience function to cancel all invokes, usefull in case of receiveing damage or death*/
        private void CancelAllInvokes()
        {
            CancelInvoke(nameof(LaunchEvilMissile));
            CancelInvoke(nameof(ActivateAnimMissile));
            CancelInvoke(nameof(LaunchMagicCircle));
            CancelInvoke(nameof(ActivateAnimMagicCircle));
            CancelInvoke(nameof(ActivateAnimTeleport));
            
        }
        /*Launches the teleport animation, then Destroys thw witch*/
        public override void OnDeath()
        {
            ActivateWitchDeathFast();
            ActivateAnimTeleport();
            /*Audio Risa*/
            Invoke(nameof(Delete),_tpTime + 0.5f);
            AllyDeath?.Invoke();
        }
        
        /*Teleports the witch on receiveing damage*/
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool facingRight = true)
        {
            if (Invulnerable) return;
            base.OnReceiveDamage(attack, facingRight);
            Debug.Log(Health.Get());
            StartHitColor();
            AccionateDamageLogic();
            Invoke(nameof(EndHitColor), InvulnerableTime);
        }

        /*Calls the delete function on this gameObject*/
        private void Delete()
        {
            Destroy(gameObject);
        }

        private void StartHitColor()
        {
            materialBruja.EnableKeyword("HITEFFECT_ON");
        }

        private void EndHitColor()
        {
            materialBruja.DisableKeyword("HITEFFECT_ON");   
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
