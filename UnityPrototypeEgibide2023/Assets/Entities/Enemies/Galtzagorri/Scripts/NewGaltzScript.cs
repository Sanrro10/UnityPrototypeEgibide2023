using System.Collections;
using Entities.Enemies.Galtzagorri.Scripts.StatePattern;
using General.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class NewGaltzScript : EntityControler
    {
        [SerializeField] private float speed = 0.05f;
        [SerializeField] public Animator animator;
        [SerializeField] public GameObject[] hideouts;
        [SerializeField] private BasicEnemyData data;
        private SpriteRenderer _spriteRenderer;
        public GameObject activeZone;
        public bool waiting;
        public GaltzStateMachine StateMachine;
        public GameObject playerGameObject;
        private bool _rotated = true;
        public Vector2 target;
        public GameObject currentHideout;
        
        private void Start()
        {
            StateMachine = new GaltzStateMachine(this);
            StateMachine.Initialize(StateMachine.GaltzHiddenState);
            Health.Set(data.health);
            playerGameObject = GameController.Instance.GetPlayerGameObject();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            InvokeRepeating(nameof(CheckDirection), 0f, 0.03f);
            InvokeRepeating(nameof(CheckPlayerPosition), 0f, 0.1f);
        }

        public void AlternateHitbox(bool state)
        {
            Rigidbody2D component = GetComponent<Rigidbody2D>();
            BoxCollider2D boxCollider2D = GetComponent<BoxCollider2D>();
            PolygonCollider2D polygonCollider2D = GetComponent<PolygonCollider2D>();
            CapsuleCollider2D capsuleCollider2D = GetComponentInChildren<CapsuleCollider2D>();
            if (state)
            {
                boxCollider2D.enabled = true;
                polygonCollider2D.enabled = true;
                capsuleCollider2D.enabled = true;
                component.isKinematic = false;
                component.simulated = true;
            }
            else
            {
                boxCollider2D.enabled = false;
                polygonCollider2D.enabled = false;
                capsuleCollider2D.enabled = false;
                component.isKinematic = true;
                component.simulated = false;
            }
        }
        
        private void CheckPlayerPosition()
        {
            if (StateMachine.CurrentState == StateMachine.GaltzRunningState && _rotated)
            {
                if (Vector3.Distance(gameObject.transform.position, playerGameObject.transform.position) < 3)
                {
                    StateMachine.TransitionTo(StateMachine.GaltzAttackState);
                }
            }
        }
        
        public void CheckDirection()
        {
            if (StateMachine.CurrentState == StateMachine.GaltzHiddenState) return;
            
            if (FacingRight && target.x < transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }
            
            if (!FacingRight && target.x > transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }
        }

        private IEnumerator Rotate()
        {
            _rotated = false;
            CancelInvoke(nameof(TurnAround));
            InvokeRepeating(nameof(TurnAround),0f, 0.1f);
            yield return new WaitUntil(() => _rotated);
            CancelInvoke(nameof(TurnAround));
        }
        
        private void TurnAround()
        {
            int newEulerY;
            if (FacingRight)
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
                FacingRight = !FacingRight;
            } 
        }

        public void FollowPlayer()
        {
            target = playerGameObject.transform.position;
        }

        public void Move()
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Floor")) return;
            if (StateMachine.CurrentState == StateMachine.GaltzAttackState)
            {
                StateMachine.TransitionTo(StateMachine.GaltzHidingState);
            }
        }

        public IEnumerator Wait()
        {
            yield return new WaitForSeconds(2f);
            if(activeZone.GetComponent<GaltzActiveZone>().isIn) StateMachine.TransitionTo(StateMachine.GaltzRunningState);
        }
        
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        {
            base.OnReceiveDamage(attack, FacingRight);
            StartCoroutine(nameof(CoInvulnerability));
        }
        
        private IEnumerator CoInvulnerability()
        {
            _spriteRenderer.material.EnableKeyword("HITEFFECT_ON");
            while (Invulnerable)
            {
                _spriteRenderer.material.SetFloat("_Alpha", 0.3f);
                                
                yield return new WaitForSeconds(0.02f);
                _spriteRenderer.material.SetFloat("_Alpha", 1f);
                yield return new WaitForSeconds(0.05f);
            }
            _spriteRenderer.material.SetFloat("_Alpha", 1f);
            _spriteRenderer.material.DisableKeyword("HITEFFECT_ON");
            yield return null;
        }

        public override void OnDeath()
        {
            StateMachine.TransitionTo(StateMachine.GaltzDeathState);
        }

        public IEnumerator Dying()
        {
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            yield return new WaitForSeconds(currentAnim.length);
            DestroyThis();
        }
        
        private void DestroyThis()
        {
            Destroy(gameObject);
        }


        /*
        // Referencia del script para los escondites
        [SerializeField] private galtzHideoutZone scriptGaltzHideout;

        // Referencia para que el Navmesh sea accesible desde todos lados
        private NavMeshAgent _navMeshAgent;

        // Referencia al RigidBody
        private Rigidbody2D _rb2D;

        // Variable para que espere X segundos al aparecer
        [SerializeField]private bool _waiting;

        // Variable para que espere X segundos hasta irse a un escondite (por si el Player está saltando, etc)
        [SerializeField]private bool _waitingForPlayer;

        // Variable para que vaya a la última posición conocida del jugador
        private Vector3 _lastAvailablePosition;

        // Variable que controla si está en proceso de esconderse
        [SerializeField]private bool _hiding;

        // Variable que controla si está escondido
        [SerializeField]private bool _hidden;

        // Variable que controla si setá atacando
        [SerializeField]private bool _attacking;

        // Referencia al animator
        [SerializeField] private Animator animator;

        // Pasar nombre de booleanos a "IDs" para ahorrarnos comparaciones de strings
        private static readonly int IsIdle1 = Animator.StringToHash("IsIdle1");
        private static readonly int IsIdle2 = Animator.StringToHash("IsIdle2");
        private static readonly int IsIdleOut = Animator.StringToHash("IsIdleOut");
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsJumping = Animator.StringToHash("IsJumping");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int IsDead = Animator.StringToHash("IsDead");


        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _rb2D = GetComponent<Rigidbody2D>();

            // Añadir una aceleración random para que no haya problemas de que se stackeen
            _navMeshAgent.acceleration = Random.Range(15, 30);

            _playerGameObject = GameController.Instance.GetPlayerGameObject();

            //Set the Health Points
            gameObject.GetComponent<HealthComponent>().SendMessage("Set", basicEnemyData.health);
        }

        // Metodo para perseguir al Player
        private void ChasePlayer()
        {
            if (CanReachPlayer())
            {
                // TODO: Animacion Run
                CancelInvoke(nameof(Hide));
                _waitingForPlayer = false;
                if (_waiting) return;
                if (_hidden)
                {
                    StartCoroutine(Appear());
                }
                else if(!_hiding)
                {
                    _navMeshAgent.SetDestination(_playerGameObject.transform.position);
                }

                if ((Vector3.Distance(gameObject.transform.position, _playerGameObject.transform.position) < 3) && !_attacking)
                {
                    StartCoroutine(nameof(Attack));
                }

            }
            else
            {
                if (!_hiding && !_hidden)
                {
                    _navMeshAgent.SetDestination(_lastAvailablePosition);
                    if (_waitingForPlayer) return;
                    _waitingForPlayer = true;
                    CancelInvoke(nameof(Hide));
                    Invoke(nameof(Hide), 2f);
                }
            }
        }

        // Metodo que elige un escondite random y va hacia el
        private void Hide()
        {
            if (_hiding || _hidden) return;
            _hiding = true;
            var placeToHide = Random.Range(0, hideouts.Length);
            if (placeToHide < 0 || placeToHide >= hideouts.Length) return;
            Vector3 whereToHide = hideouts[placeToHide].transform.position;
            Debug.Log(whereToHide);
            _navMeshAgent.SetDestination(whereToHide);
        }

        // Metodo que elige un escondite random para aparecer
        private IEnumerator Appear()
        {
            _waiting = true;
            yield return new WaitForSeconds(2f);
            _navMeshAgent.isStopped = false;
            _waiting = false;
            int placeToAppear = Random.Range(0, hideouts.Length);
            if (placeToAppear < 0 || placeToAppear >= hideouts.Length) yield break;
            gameObject.transform.position = hideouts[placeToAppear].transform.position;

            // TODO: Animacion Salir del Escondite

            _hidden = false;
            _hiding = false;
        }

        // Metodo para atacar
        private IEnumerator Attack()
        {
            if (!_hidden && !_hiding)
            {
                _attacking = true;
                CancelInvoke(nameof(ChasePlayer));
                _navMeshAgent.isStopped = true;
                _navMeshAgent.enabled = false;

                // TODO: Animacion Ataque

                _rb2D.velocity = new Vector2(0, 0);
                _rb2D.AddForce(new Vector2((_playerGameObject.transform.position.x - transform.position.x) * 2, 5), ForceMode2D.Impulse);
                yield return new WaitUntil(() => !_attacking);
                _rb2D.velocity = new Vector2(0, 0);
                ActivateEnemy();
                Hide();
                yield return new WaitForSeconds(1f);
            }
        }

        // Metodo que comprueba si el Player es accesible
        private bool CanReachPlayer()
        {
            if (_navMeshAgent.enabled)
            {
                NavMeshPath path = new NavMeshPath();
                if (_navMeshAgent.CalculatePath(_playerGameObject.transform.position, path))
                {
                    if (path.status == NavMeshPathStatus.PathComplete)
                    {
                        _lastAvailablePosition = _playerGameObject.transform.position;
                        return true;
                    }
                }
                return false;
            }
            return false;
        }

        // Metodo que activa el meterse al escondite
        public void ActivateHiding(Collider2D other)
        {
            if (Vector3.Distance(other.gameObject.transform.position, _navMeshAgent.destination) < 1 && _hiding)
            {
                Debug.Log("ACTIVATE HIDING");
                _navMeshAgent.isStopped = true;
                _rb2D.velocity = new Vector2(0, 0);
                _hiding = false;
                _hidden = true;
            }
        }

        // Metodo que activa el enemigo
        public void ActivateEnemy()
        {
            // TODO: Animacion Salir del Escondite
            CancelInvoke(nameof(ChasePlayer));
            InvokeRepeating(nameof(ChasePlayer), 0f, 0.1f);
            scriptGaltzHideout.ResetHit();
        }

        // Metodo que desactiva el enemigo
        public void DeactivateEnemy()
        {
            CancelInvoke(nameof(ChasePlayer));
            Hide();
        }

        public override void OnDeath()
        {
            // TODO Logica de muerte relacionada
            animator.SetBool(IsIdle1, false);
            animator.SetBool(IsHurt, false);
            animator.SetBool(IsIdle2, false);
            animator.SetBool(IsRunning, false);
            animator.SetBool(IsJumping, false);
            animator.SetBool(IsIdleOut, false);
            animator.SetBool(IsDead, true);

            Invoke(nameof(DestroyThis),2f);
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Floor")) return;
            if (_attacking)
            {
                _navMeshAgent.enabled = true;
                _navMeshAgent.isStopped = false;
                _attacking = false;
            }

        }*/
    }
}
