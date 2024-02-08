using System;
using System.Collections;
using System.Collections.Generic;
using Entities;
using UnityEngine;

public class DamageInteractableEmitter : EntityControler
{
    public static event Action<int> OnActivate;
    
    [SerializeField]
    private AttackComponent.AttackType damageType;
    
    [SerializeField]
    private int id;

    private Animator _animator;
    void Start()
    {
        _animator ??= GetComponent<Animator>();
    }

    public override void OnReceiveDamage(AttackComponent.AttackData attackData, bool toTheRight = true)
    {
        if (attackData.attackType == damageType && Health.Get() > 0)
        {
            _animator.SetTrigger("Activate");
            OnActivate?.Invoke(id);
        }
    }
    
}
