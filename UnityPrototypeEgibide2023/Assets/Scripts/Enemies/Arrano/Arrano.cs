using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Arrano : EntityControler
{
    // Variable para controlar la velocidad
    [SerializeField] private float flyingSpeed;
    
    // Referencia al jugador
    [SerializeField] private GameObject player;
    
    // Datos del enemigo
    [SerializeField] private FlyingEnemyData flyingEnemyData;
    
    // Layer del Player
    [SerializeField] private LayerMask playerLayer;
    
    // Cooldown del ataque
    [SerializeField] private float attackCooldown;
    
    // Posiciones entre las que vuela
    private Vector3 _leftLimitPosition;
    private Vector3 _rightLimitPosition;

    // Variable que controla la direccion
    private bool _facingRight = true;
    
    // Variable que controla si te puede pegar
    private bool _hit;
    
    // Posiciones a la hora de atacar
    private Vector3 _direction;
    private Vector3 _startPosition;
    private Vector3 _endPosition;
    
    private float _yPos;
    private float _posX;
    // Start is called before the first frame update
    void Start()
    {
        var rightLimit = gameObject.transform.Find("RightLimit");
        _rightLimitPosition = rightLimit.position;
        
        var leftLimit = gameObject.transform.Find("LeftLimit");
        _leftLimitPosition = leftLimit.position;
       
        _direction = _rightLimitPosition;
        _yPos = transform.position.y;
        
        //Set the Health Points
        gameObject.GetComponent<HealthComponent>().SendMessage("Set",flyingEnemyData.health, SendMessageOptions.RequireReceiver);
        
        InvokeRepeating(nameof(PassiveBehaviour),0,0.01f);
        InvokeRepeating(nameof(LookForPlayer),0,0.1f);
        InvokeRepeating(nameof(ChangeVertical),0.5f,1f);
    }

    // Metodo para que se mueva entre dos puntos
    void PassiveBehaviour()
    {
        
        // TODO: Animacion Idle
        
        if (Math.Abs(transform.position.x - _leftLimitPosition.x) < 0.5)
        {
            _facingRight = true;
            _direction = _rightLimitPosition;
            //StartCoroutine(nameof(TurnAround));
            TurnAround();
        }
        
        if (Math.Abs(transform.position.x - _rightLimitPosition.x) < 0.5)
        {
            _direction = _leftLimitPosition;
            _facingRight = false;
            //StartCoroutine(nameof(TurnAround));
            TurnAround();
        }
        
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(_direction.x, _yPos), flyingSpeed);
    }

    // Metodo que cambia un poco la altura para que el movimiento no sea una linea recta
    void ChangeVertical()
    {
        _yPos = _direction.y + Random.Range(-1, 3);
    }
    
    // Metodo que lanza Raycast en un angulo de 45º hasta que ve al jugador y lo ataca
    private void LookForPlayer()
    {
        RaycastHit2D hitData;
        if (_facingRight)
        {
            hitData = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector3(0.5f, -0.5f)), 50, playerLayer);
        }
        else
        {
            hitData = Physics2D.Raycast(transform.position, transform.TransformDirection(new Vector3(-0.5f, -0.5f)), 50, playerLayer);
        }

        if (hitData.collider == null) return;
        if (hitData.collider.CompareTag("Player"))
        {
            Attack();
        }

    }
    
    // Metodo con la logica del ataque
    public void Attack()
    {
        CancelInvoke(nameof(PassiveBehaviour));
        CancelInvoke(nameof(ChangeVertical));
        CancelInvoke(nameof(LookForPlayer));
        _startPosition = transform.position;
        _endPosition = player.transform.position;

        StartCoroutine(nameof(GoDown));
    }
    
    // Metodo para move el enemigo a una posicion
    private void Move(Vector2 origin, Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(origin, target, speed);
    }
    
    // Corutina para ir hacia abajo hasta que llega al Y del jugador
    private IEnumerator GoDown()
    {
        
        // TODO: Animacion Picado
        
        _posX = transform.position.x;
        InvokeRepeating(nameof(StartMovingDown),0,0.01f);
        yield return new WaitUntil(() => Math.Abs(transform.position.y - _endPosition.y) < 0.5f);
        CancelInvoke(nameof(StartMovingDown));
        StartCoroutine(nameof(GoTowards));
    }
    
    // Metodo que se mueva hacia abajo
    private void StartMovingDown()
    {
        Move(transform.position, new Vector2(_posX, _endPosition.y), flyingSpeed * 1.2f);
    }
    
    // Corutina para ir en linea recta hasta atravesar al jugador
    private IEnumerator GoTowards()
    {
        
        // TODO: Animacion Idle
        
        InvokeRepeating(nameof(StartMovingTowards), 0, 0.01f);
        if (_facingRight)
        {
            yield return new WaitUntil(() => Math.Abs((transform.position.x - 10) - _endPosition.x) <= 0.5f);
        }
        else
        {
            yield return new WaitUntil(() => Math.Abs(_endPosition.x - (transform.position.x + 10)) <= 0.5f);
        }
        
        CancelInvoke(nameof(StartMovingTowards));
        StartCoroutine(nameof(GoUp));
    }

    // Metodo que se mueva en linea recta hacia el jugador
    private void StartMovingTowards()
    {
        if (_facingRight)
        {
            Move(transform.position, new Vector2(_endPosition.x * 1.5f, transform.position.y), flyingSpeed);
        }
        else
        {
            Move(transform.position, new Vector2(_endPosition.x * -1.5f, transform.position.y), flyingSpeed);
        }
        
    }
    
    // Corutina que vuelve al Y inicial
    private IEnumerator GoUp()
    {
        
        // TODO: Animacion subida
        
        InvokeRepeating(nameof(StartMovingUp),0,0.01f);
        yield return new WaitUntil(() => Math.Abs(transform.position.y - _startPosition.y) < 0.5f);
        CancelInvoke(nameof(StartMovingUp));
        InvokeRepeating(nameof(PassiveBehaviour),0,0.01f);
        InvokeRepeating(nameof(ChangeVertical),2f,1f);
        StartCoroutine(nameof(AttackCooldown));
    }
    
    // Metodo para que se vuelva hacia arriba
    private void StartMovingUp()
    {
        Move(transform.position, new Vector2(transform.position.x, _startPosition.y), flyingSpeed * 1.2f);
    }
    
    // Corutina que espera X segundos hasta que vuelve a checkear si ataca al jugador
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _hit = false;
        InvokeRepeating(nameof(LookForPlayer),0,0.1f);
    }
        
    
    // Corutina para darse la vuelta
    // TODO: HACERLA NO FUNCIONA
    /*private IEnumerator TurnAround()
    {

        /*while(Mathf.Abs(this.transform.rotation.eulerAngles.y - (_facingRight ? 180 : 0)) > 0.3f)
        {
            this.transform.Rotate(new Vector3(0, 1, 0), 180f * Time.deltaTime);
            yield return Time.deltaTime;
        }#1#
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        yield return new WaitForSeconds(0.1f);
    }*/

    // Metodo temporal que hace que se de la vuelta (bruscamente)
    private void TurnAround()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public override void OnDeath()
    {
        //TODO Animacion Muerte
        
        Invoke(nameof(DestroyThis),2f);
        
    }

    private void DestroyThis()
    {
        Destroy(gameObject);
    }

    // Evento que hace que el Player reciba daño
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hit)
        {
            player.GetComponent<PlayerController>().ReceiveDamage(1);
            _hit = true;
        }
    }
}
