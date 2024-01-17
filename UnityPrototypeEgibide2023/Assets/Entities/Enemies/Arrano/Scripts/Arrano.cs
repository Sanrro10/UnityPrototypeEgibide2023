using System;
using System.Collections;
using System.Xml.Schema;
using Entities.Player.Scripts;
using General.Scripts;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Entities.Enemies.Arrano.Scripts
{
    public class Arrano : EntityControler
    {
        // Referencia a Audio Source
        [SerializeField] private AudioSource _audioSource;
        // Referencia a Animator
        [SerializeField] private Animator _animator;
        
        // Referencia al sistema de particulas
        [SerializeField] private ParticleSystem plumasMuerte;
        
        // Referencia a los audios
        [SerializeField] private Audios audioData;
        
        // Variable para controlar la velocidad
        [SerializeField] private float flyingSpeed;
        private float _originalFS;
    
        // Referencia al jugador
        private GameObject _player;
    
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

        private int _tiempoTotal = 30;
        private int _tiempoAudioIdle = 0;
        private int _tiempoAudioAttack = 0;
        private int _tiempoAudioUp = 0;

        private char _Idle;
        private char _Attack;
        private char _Up;
        // Start is called before the first frame update
        void Start()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            
            var rightLimit = gameObject.transform.Find("RightLimit");
            _rightLimitPosition = rightLimit.position;
        
            var leftLimit = gameObject.transform.Find("LeftLimit");
            _leftLimitPosition = leftLimit.position;
       
            _direction = _rightLimitPosition;
            _yPos = transform.position.y;

            _originalFS = flyingSpeed;

            _facingRight = true;
            
            _player = GameController.Instance.GetPlayerGameObject();
        
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
            Audios(1, _tiempoTotal, _tiempoAudioIdle, 'I');
            
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
                //Debug.DrawRay(transform.position, Vector2.down + Vector2.right, Color.red, 3f);
            }
            else
            {
                hitData = Physics2D.Raycast(transform.position, Vector2.down + Vector2.left, 50, playerLayer);
                //Debug.DrawRay(transform.position, Vector2.down + Vector2.left, Color.blue, 3f);
            }
            
            if (hitData.collider == null) return;
            Debug.Log(hitData.collider.tag);
            if (hitData.collider.CompareTag("Player"))
            {
                _animator.SetBool("IsPreAttack", true);
                
                Attack();
            }

        }
    
        // Metodo con la logica del ataque
        public void Attack()
        {
            Audios(0, _tiempoTotal, _tiempoAudioAttack, 'A');
            
            CancelInvoke(nameof(PassiveBehaviour));
            CancelInvoke(nameof(ChangeVertical));
            CancelInvoke(nameof(LookForPlayer));
            _startPosition = transform.position;
            _endPosition = _player.transform.position;
            
            //_animator.SetBool("IsPreAttack", false);
            
            //Debug.Log(_endPosition);
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
            _animator.SetBool("IsAttack", true);
            _animator.SetBool("IsPreAttack", false);

            _posX = transform.position.x;
            InvokeRepeating(nameof(StartMovingDown),0,0.01f);
            yield return new WaitUntil(() => Math.Abs(transform.position.y - _endPosition.y) < 0.1f);
            //yield return new WaitUntil(() => transform.position.y < _endPosition.y);
            CancelInvoke(nameof(StartMovingDown));
            StartCoroutine(nameof(GoTowards));
        }
    
        // Metodo que se mueva hacia abajo
        private void StartMovingDown()
        {
            Debug.Log(transform.position.y + " --- " + _endPosition.y);
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
            
            _animator.SetBool("IsAttack", false);
            
            StartCoroutine(nameof(GoUp));
        }

        // Metodo que se mueva en linea recta hacia el jugador
        private void StartMovingTowards()
        {
            Debug.Log(_endPosition.x - (transform.position.x + 8));
            if (_facingRight)
            {
                Move(transform.position, new Vector2(_endPosition.x + 50, transform.position.y), flyingSpeed * 0.8f);
            }
            else
            {
                Move(transform.position, new Vector2(_endPosition.x - 50, transform.position.y), flyingSpeed * 0.8f);
            }
        
        }
    
        // Corutina que vuelve al Y inicial
        private IEnumerator GoUp()
        {
            
            // TODO: Animacion subida
            _animator.SetBool("IsUp", true);
            Audios(2, _tiempoTotal, _tiempoAudioUp, 'U');
        
            InvokeRepeating(nameof(StartMovingUp),0,0.01f);
            yield return new WaitUntil(() => Math.Abs(transform.position.y - _startPosition.y) < 0.5f);
            CancelInvoke(nameof(StartMovingUp));
            ChangeDirection();
            InvokeRepeating(nameof(PassiveBehaviour),0,0.01f);
            InvokeRepeating(nameof(ChangeVertical),2f,1f);
            StartCoroutine(nameof(AttackCooldown));
            
            _animator.SetBool("IsUp", false);
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
            _audioSource.clip = audioData.audios[1];
            _audioSource.Play();
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

        public void Audios(int audio, int tiempoAudioTotal, int tiempoAudioModo, char tipo)
        {
        switch (tipo)
        {
            case 'I':
                if (tiempoAudioTotal == 0 && tiempoAudioModo == 0 )
                {
                    _audioSource.clip = audioData.audios[audio];
                    _audioSource.Play();

                    _tiempoAudioIdle = 30;
                    _tiempoTotal = 30;
                }
                else
                {
                    Timer();
                    
                }

                break;
            case 'A':
                if ( tiempoAudioModo == 0)
                {
                    _audioSource.clip = audioData.audios[audio];
                    _audioSource.Play();

                    _tiempoAudioAttack = 30;
                    _tiempoTotal = 20;
                }
                else
                {
                    Timer();
                }
                
                break;
            case 'U':
                if (tiempoAudioModo == 0)
                {
                    _audioSource.clip = audioData.audios[audio];
                    _audioSource.Play();

                    _tiempoAudioUp = 30;
                    _tiempoTotal = 20;
                }
                else
                {
                    Timer();
                }
                break;
            default:  
                break;
        }
        
        void Timer()
        {
            if (_tiempoAudioAttack >= 1)
            {
                _tiempoAudioAttack -= 1;
            } else if (_tiempoAudioIdle >= 1)
            {
                _tiempoAudioIdle -= 1;
            } else if (_tiempoAudioUp >= 1)
            {
                _tiempoAudioUp -= 1;
            }    
        }        
        
        }

        public override void OnDeath()
        {
            //TODO Animacion Muerte
            _animator.SetBool("IsDead", true );
            Instantiate(plumasMuerte, transform.position, transform.rotation);
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
                _player.GetComponent<PlayerController>().OnReceiveDamage(1, 0, Vector2.zero);
                _hit = true;
            }
        }
    }
}
