using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.UI;


public class PlayerCombat : MonoBehaviour
{
    /*This script manage all things melee combat
     including but not limited to:
     The position of the sword when attacking*/
    
    /*--------VARIABLES----------*/
    private InputActions _controls;

    [SerializeField] private Animator _animator;

    private GameObject _player;
    private bool _onPotionColdown;
    private Slider _sliderPotion;
    private float _sliderPotionValue;
    private PlayerData _playerData;

    private ConstraintSource _playerConstraintSource;
    private Vector3 _leftAttackPos = new Vector3(-1f,0,0);
    private Vector3 _rightAttackPos = new Vector3(1f,0,0);
    private Vector3 _neutralPos = Vector3.zero;

    private PlayerController _playerController;

    public GameObject potion;
    
    /*-----------MAIN FUNCTIONS------------------*/
    void Start()
    {
        _controls = new InputActions();
        _player = GameObject.Find("Player Espada State");
        _animator = _player.GetComponent<Animator>();
        
        _playerController = _player.GetComponent<PlayerController>();
        _playerData = _playerController.GetPlayerData();
        
        _sliderPotion = GameObject.Find("SliderPotion").GetComponent<Slider>();
        _sliderPotion.maxValue = _playerData.potionColdownTime;
        _sliderPotionValue = _playerData.potionColdownTime;
        
        //Sets the parent to be the player
        _playerConstraintSource.sourceTransform = _player.transform;
        _playerConstraintSource.weight = 1;
        gameObject.GetComponent<ParentConstraint>().SetSource(0,_playerConstraintSource);
        
        
        _controls.Enable();
       
        //Melee Attack
        _controls.GeneralActionMap.Attack.performed += ctx => Attack();
        
        //Potion launch
        _controls.GeneralActionMap.Potion.performed += ctx => Potion();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*-------------CUSTOM FUNCTIONS-----------------*/
    void Attack()
    {
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") 
            || _animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")
            || _animator.GetCurrentAnimatorStateInfo(0).IsName("OnAir"))
        {
            Debug.Log("ATACOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
            
            //The direction of the sprite is checked to attack in that direction
            if (_player.GetComponent<SpriteRenderer>().flipX == true)//left
            {
                gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0,_leftAttackPos);
                
            }else if (_player.GetComponent<SpriteRenderer>().flipX == false)//right
            {
                gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0,_rightAttackPos);
            }
            _animator.SetTrigger("MeleeAttack");
            StartCoroutine(mockAttack());
            //gameObject.GetComponent<ParentConstraint>().SetTranslationOffset(0,_neutralPos);
        }
    }
    
    void Potion()
    {
        if (!_onPotionColdown)
        {
            Debug.Log("POTION LAUNCH");
            Instantiate(potion, new Vector2(_player.transform.position.x, _player.transform.position.y + 2), Quaternion.identity);
            _onPotionColdown = true;
            _sliderPotion.value = 0;
            Invoke(nameof(PotionCooldown), _playerData.potionColdownTime);
            InvokeRepeating(nameof(PotionCooldownSlider), 0f, 1f);
        }
    }
        
        
    public void PotionCooldown()
    {
        _onPotionColdown = false;
        _sliderPotionValue = _playerData.potionColdownTime;
        CancelInvoke(nameof(PotionCooldownSlider));
    }

    public void PotionCooldownSlider()
    {
        _sliderPotion.value += 1;
    }
        
    
    /*---------------EVENTOS---------------*/
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Debug.Log("LE HE DADO");
            //Logic for damaging enemies
            // other.gameObject.RestarUnaVida();
            // if (other.gameObject.vidas <= 0)
            // {
            //     //Reward Logic, if its needed, in another function
            //     //Maybe it would be nice to call another class (ej: Enemy), to handle death
            //     //actions on that part, before destroying
            //     Destroy(other.gameObject);
            //     //Maybe the destroy is triggered inside the enemy proper
            // 
            other.gameObject.GetComponent<HealthComponent>().SendMessage("RemoveHealth", 1);

        }
    }
    
    /*-------------COROUTINES---------------*/
    
    //DELETE LATER
    private IEnumerator mockAttack()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        GetComponent<BoxCollider2D>().enabled = false;
        
    }
}
