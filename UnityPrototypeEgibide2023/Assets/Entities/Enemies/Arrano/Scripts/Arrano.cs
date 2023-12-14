using System;
using System.Collections;
using Entities.Player.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Enemies.Arrano.Scripts
{
    public class Arrano : EntityControler
    {
        // Variable para controlar la velocidad
        [SerializeField] private float flyingSpeed;
        private float _originalFS;
    
        // Referencia al jugador
        [SerializeField] private GameObject player;
    
        // Datos del enemigo
        [SerializeField] private FlyingEnemyData flyingEnemyData;
    
        // Layer del Player
        [SerializeField] private LayerMask playerLayer;
    
        // Cooldown del ataque
        [SerializeField] private float attackCooldown;
    
        // Variable que controla la rotación
        private bool _rotated;
    
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

            _originalFS = flyingSpeed;

            _facingRight = true;
        
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
        
            if (!_facingRight && Math.Abs(transform.position.x - _leftLimitPosition.x) < 0.5)
            {
                _facingRight = true;
                _direction = _rightLimitPosition;
                StartCoroutine(nameof(Rotate));
            }
        
            if (_facingRight && Math.Abs(transform.position.x - _rightLimitPosition.x) < 0.5)
            {
                _facingRight = false;
                _direction = _leftLimitPosition;
                StartCoroutine(nameof(Rotate));
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
                hitData = Physics2D.Raycast(transform.position, Vector2.down + Vector2.right, 50, playerLayer);
                Debug.DrawRay(transform.position, Vector2.down + Vector2.right, Color.red, 3f);
            }
            else
            {
                hitData = Physics2D.Raycast(transform.position, Vector2.down + Vector2.left, 50, playerLayer);
                Debug.DrawRay(transform.position, Vector2.down + Vector2.left, Color.blue, 3f);
            }

            if (hitData.collider == null) return;
            Debug.Log(hitData.collider.tag);
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
            if (_facingRight)
            {
                Move(transform.position, new Vector2(transform.position.x + 3, _endPosition.y), flyingSpeed * 1.2f);
            }
            else
            {
                Move(transform.position, new Vector2(transform.position.x - 3, _endPosition.y), flyingSpeed * 1.2f);
            }
        
        }
    
        // Corutina para ir en linea recta hasta atravesar al jugador
        private IEnumerator GoTowards()
        {
        
            // TODO: Animacion Idle
        
            InvokeRepeating(nameof(StartMovingTowards), 0, 0.01f);
            if (_facingRight)
            {
                yield return new WaitUntil(() => Math.Abs((transform.position.x - 8) - _endPosition.x) <= 0.5f);
            }
            else
            {
                yield return new WaitUntil(() => Math.Abs(_endPosition.x - (transform.position.x + 8)) <= 0.5f);
            }
        
            CancelInvoke(nameof(StartMovingTowards));
            StartCoroutine(nameof(GoUp));
        }

        // Metodo que se mueva en linea recta hacia el jugador
        private void StartMovingTowards()
        {
            if (_facingRight)
            {
                Move(transform.position, new Vector2(_endPosition.x * 1.5f, transform.position.y), flyingSpeed * 0.8f);
            }
            else
            {
                Move(transform.position, new Vector2(_endPosition.x * -1.5f, transform.position.y), flyingSpeed * 0.8f);
            }
        
        }
    
        // Corutina que vuelve al Y inicial
        private IEnumerator GoUp()
        {
        
            // TODO: Animacion subida
        
            InvokeRepeating(nameof(StartMovingUp),0,0.01f);
            yield return new WaitUntil(() => Math.Abs(transform.position.y - _startPosition.y) < 0.5f);
            CancelInvoke(nameof(StartMovingUp));
            ChangeDirection();
            InvokeRepeating(nameof(PassiveBehaviour),0,0.01f);
            InvokeRepeating(nameof(ChangeVertical),2f,1f);
            StartCoroutine(nameof(AttackCooldown));
        }

        private void ChangeDirection()
        {
            if (_facingRight)
            {
                _facingRight = false;
                _direction = _leftLimitPosition;
            }
            else
            {
                _facingRight = true;
                _direction = _rightLimitPosition;

            }
            StartCoroutine(nameof(Rotate));
        }
    
        // Metodo para que se vuelva hacia arriba
        private void StartMovingUp()
        {
            float newEndPosition;
        
            if (_facingRight)
            {
                newEndPosition = _endPosition.x + (_endPosition.x - _startPosition.x);
            }
            else
            {
                newEndPosition = _endPosition.x - (_startPosition.x - _endPosition.x);
            }
        
            Move(transform.position, new Vector2(newEndPosition, transform.position.y + 3), flyingSpeed * 0.6f);
        
        }
    
        // Corutina que espera X segundos hasta que vuelve a checkear si ataca al jugador
        private IEnumerator AttackCooldown()
        {
            yield return new WaitForSeconds(attackCooldown);
            _hit = false;
            InvokeRepeating(nameof(LookForPlayer),0,0.1f);
        }
        
    
        // Corutina para girar
        private IEnumerator Rotate()
        {
            flyingSpeed *= 0.3f;
            _rotated = false;
            CancelInvoke(nameof(TurnAround));
            InvokeRepeating(nameof(TurnAround),0f, 0.1f);
            yield return new WaitUntil(() => _rotated);
            CancelInvoke(nameof(TurnAround));
            flyingSpeed = _originalFS;
        }

        // Metodo temporal que hace que se de la vuelta (bruscamente)
        private void TurnAround()
        {

            int newEulerY;
            if (_facingRight)
            {
                transform.eulerAngles = new Vector3(transform.transform.eulerAngles.x, transform.rotation.eulerAngles.y - 30, transform.rotation.eulerAngles.z);
                newEulerY = (int)transform.rotation.eulerAngles.y;
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.transform.eulerAngles.x, transform.rotation.eulerAngles.y + 30, transform.rotation.eulerAngles.z);
                newEulerY = (int)transform.rotation.eulerAngles.y;
            }
        
            if (newEulerY % 180 == 0 || newEulerY == 0)
            {
                _rotated = true;
            } 
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
}
