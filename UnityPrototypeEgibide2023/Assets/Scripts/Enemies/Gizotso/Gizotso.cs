using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Gizotso : EntityControler
{

    [SerializeField] private PassiveEnemyData passiveEnemyData;
    public bool attacking;
    private Vector3 _leftLimitPosition;
    private Vector3 _rightLimitPosition;
    private bool _goingRight = true;
    private NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    void Start()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        
        var rightLimit = gameObject.transform.Find("RightLimit");
        _rightLimitPosition = rightLimit.position;

        var leftLimit = gameObject.transform.Find("LeftLimit");
        _leftLimitPosition = leftLimit.position;
        
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set", passiveEnemyData.health);
        
        InvokeRepeating(nameof(PassiveBehavior), 0f, 0.1f);
    }
    
    private void PassiveBehavior()
    {
        
        // TODO: Animacion Walk
        
        if (attacking) return;
        _navMeshAgent.SetDestination(_goingRight ? _rightLimitPosition : _leftLimitPosition);
        if (Math.Abs(transform.position.x - _leftLimitPosition.x) < 0.5)
        {
            _goingRight = true;
        }
        
        if (Math.Abs(transform.position.x - _rightLimitPosition.x) < 0.5)
        {
            _goingRight = false;
        }
    }

    public void Attack()
    {
        if (!attacking)
        {
            StartCoroutine(nameof(Cooldown));
        }
    }

    private IEnumerator Cooldown()
    {
        Debug.Log("ENTRA EN RANGO");
        _navMeshAgent.isStopped = true;
        attacking = true;

        Debug.Log("PRE ATAQUE");
        // TODO: Animacion Pre-Ataque
        
        yield return new WaitForSeconds(1f);
        
        Debug.Log("ATAQUE");
        // TODO: Animacion Ataque

        GameObject attackZone = gameObject.transform.Find("AttackZone").gameObject;
        attackZone.SetActive(true);
        GetComponentInChildren<GizotsoAttackZone>().Attack();
        yield return new WaitForSeconds(1.5f);
        attackZone.SetActive(true);
        GetComponentInChildren<GizotsoAttackZone>().Attack();
        yield return new WaitForSeconds(0.5f);
        _navMeshAgent.isStopped = false;
        
        yield return new WaitForSeconds(3f);
        attacking = false;
    }
    
    public override void OnDeath()
    {
        // TODO Animacion Muerte
        
        Invoke(nameof(DestroyThis),2f);
        
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

}
