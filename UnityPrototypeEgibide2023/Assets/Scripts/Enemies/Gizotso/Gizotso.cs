using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Gizotso : EntityControler
{
    // Datos del enemigo
    [SerializeField] private PassiveEnemyData passiveEnemyData;
    
    // Variable para que no ande ni entre en bucle de ataques mientras está atacando
    public bool attacking;
    
    // Posiciones de los límites entre los que andas
    private Vector3 _leftLimitPosition;
    private Vector3 _rightLimitPosition;
    
    // Variable para controlar la direccion
    private bool _goingRight = true;
    
    // Variable para acceder al Navmesh desde todos lados
    private NavMeshAgent _navMeshAgent;
    
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
            return;
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
        _navMeshAgent.isStopped = true;
        attacking = true;
        
        // TODO: Animacion Pre-Ataque
        yield return new WaitForSeconds(1f);
        
        // TODO: Animacion Ataque
        GameObject attackZone = gameObject.transform.Find("AttackZone").gameObject;
        
        // TODO --------------------------------------------------------------------------
        // CUADRAR TIEMPOS CON LA ANIMACION
        
        // Primer golpe 
        attackZone.SetActive(true);
        InvokeRepeating(nameof(Dash),0f,0.05f);
        GetComponentInChildren<GizotsoAttackZone>().Attack();
        yield return new WaitForSeconds(0.5f);
        CancelInvoke(nameof(Dash));
        
        // Segundo golpe
        attackZone.SetActive(true);
        InvokeRepeating(nameof(Dash),0f,0.05f);
        GetComponentInChildren<GizotsoAttackZone>().Attack();
        yield return new WaitForSeconds(0.5f);
        CancelInvoke(nameof(Dash));
        
        // TODO: Animacion de stun
        // TODO: Tiempo que va a estar stuneado
        yield return new WaitForSeconds(2f);
        
        // TODO: Animacion idle
        
        _navMeshAgent.isStopped = false;
        
        // Tiempo hasta que puede hacer otro ataque para que no se quede en bucle atacando
        yield return new WaitForSeconds(3f);
        attacking = false;
    }

    private void Dash()
    {
        if (_goingRight)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 0.1f, transform.position.y), 0.1f);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 0.1f, transform.position.y), 0.1f);
        }
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
