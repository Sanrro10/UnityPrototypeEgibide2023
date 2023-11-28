using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class Arrano : EntityControler
{
    [SerializeField] private float flyingSpeed;
    [SerializeField] private GameObject player;
    [SerializeField] private FlyingEnemyData flyingEnemyData;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown;
    private Vector3 _leftLimitPosition;
    private Vector3 _rightLimitPosition;

    private bool _facingRight = true;
    private bool _hit;
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

    void ChangeVertical()
    {
        _yPos = _direction.y + Random.Range(-1, 3);
    }

    private void Move(Vector2 origin, Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(origin, target, speed);
    }

    private void StartMovingDown()
    {
        Move(transform.position, new Vector2(_posX, _endPosition.y), flyingSpeed * 1.2f);
    }

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

    private void StartMovingUp()
    {
        Move(transform.position, new Vector2(transform.position.x, _startPosition.y), flyingSpeed * 1.2f);
    }

    public void Attack()
    {
        CancelInvoke(nameof(PassiveBehaviour));
        CancelInvoke(nameof(ChangeVertical));
        CancelInvoke(nameof(LookForPlayer));
        _startPosition = transform.position;
        _endPosition = player.transform.position;

        StartCoroutine(nameof(GoDown));
    }

    private IEnumerator GoDown()
    {
        
        // TODO: Animacion Picado
        
        _posX = transform.position.x;
        InvokeRepeating(nameof(StartMovingDown),0,0.01f);
        yield return new WaitUntil(() => Math.Abs(transform.position.y - _endPosition.y) < 0.5f);
        CancelInvoke(nameof(StartMovingDown));
        StartCoroutine(nameof(GoTowards));
    }

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

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackCooldown);
        _hit = false;
        InvokeRepeating(nameof(LookForPlayer),0,0.1f);
    }
        
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hit)
        {
            player.GetComponent<PlayerController>().ReceiveDamage(1);
            _hit = true;
        }
    }
}
