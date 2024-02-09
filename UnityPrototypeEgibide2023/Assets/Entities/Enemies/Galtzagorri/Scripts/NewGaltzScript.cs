using System.Collections;
using Entities.Enemies.Galtzagorri.Scripts.StatePattern;
using General.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class NewGaltzScript : EntityControler
    {
        // Velocidad
        [SerializeField] private float speed = 0.05f;
        
        // Array de escondites
        [SerializeField] public GameObject[] hideouts;
        
        // Datos del galtzagorri
        [SerializeField] private BasicEnemyData data;
        
        // Referencia al objeto con que hace daño
        [SerializeField] public GameObject attackZone;
        
        // Referencia al escondite inicial
        [SerializeField] public GameObject startHideout;
        
        // Referencia a su zona de activación
        [SerializeField] public GameObject activeZone;
        
        // Variable para controlar que esté 2 segundos esperando para salir del escondite
        public bool waiting;
        
        // Referencia a la State Machine
        public GaltzStateMachine StateMachine;
        
        // Referencia al animator
        public Animator animator;
        
        // Referencia al player
        public GameObject playerGameObject;
        
        // Variable para controlar hacia donde tiene que ir
        public Vector2 target;
        
        // Referencia al escondite actual
        public GameObject currentHideout;
        
        // Referencia al sprite renderer
        private SpriteRenderer _spriteRenderer;
        
        // Variable para controlar la rotación
        private bool _rotated = true;
        
        // Variable que controla si el player se ha alejado suficiente de su escondite para poder salir
        public bool canExit = true;
        
        // Variable que controla si el player está dentro de la zona de activación
        public bool isIn;
        
        // variables para reducir las comparaciones con strings
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        private void Start()
        {
            // Iniciar la State Machine y ponerle en modo "Hiding" para que se esconda en el escondite inicial
            StateMachine = new GaltzStateMachine(this);
            StateMachine.Initialize(StateMachine.GaltzHidingState);
            PlaceToHide(startHideout);
            
            // Iniciar sus datos
            Health.Set(data.health);
            
            // Suscribirse a los actions de la zona de activación y del escondite
            GaltzHideoutRange.PlayerEntered += PlayerEntered;
            GaltzHideoutRange.PlayerExited += PlayerExited;
            GaltzActiveZone.PlayerEnteredArea += PlayerEnteredArea;
            GaltzActiveZone.PlayerExitedArea += PlayerExitedArea;
            
            // Coger la referencia del jugador de la escena
            playerGameObject = GameController.Instance.GetPlayerGameObject();
            
            // Coger la referencia del sprite
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            // Iniciar metodos para comprobar si tiene que girar y para ver donde está el player
            InvokeRepeating(nameof(CheckDirection), 0f, 0.03f);
            InvokeRepeating(nameof(CheckPlayerPosition), 0f, 0.01f);
        }

        // Metodo para cuando el escondite lanza el evento de que el player ha entrado
        private void PlayerEntered(GameObject hideout)
        {
            if (hideout.Equals(currentHideout))
            {
                canExit = false;
            }
        }

        // Metodo para cuando el escondite lanza el evento de que el player ha salido
        private void PlayerExited(GameObject hideout)
        {
            if (hideout.Equals(currentHideout))
            {
                canExit = true;
            }
        }

        // Metodo para cuando la zona de activación lanza el evento de que el player ha entrado
        private void PlayerEnteredArea(GameObject area)
        {
            if (area.Equals(activeZone))
            {
                isIn = true;
                if (canExit)
                {
                    if (StateMachine.CurrentState == StateMachine.GaltzHiddenState)
                    {
                        StateMachine.TransitionTo(StateMachine.GaltzRunningState);
                    }
                }
            }
        }

        // Metodo para cuando la zona de activación lanza el evento de que el player ha salido
        private void PlayerExitedArea(GameObject area)
        {
            if (area.Equals(activeZone))
            {
                isIn = false;
                if (StateMachine.CurrentState == StateMachine.GaltzRunningState ||
                    StateMachine.CurrentState == StateMachine.GaltzAttackState)
                {
                    StateMachine.TransitionTo(StateMachine.GaltzHidingState);
                }
            }
        }

        // Metodo para desactivar colisiones y que no se caiga de la escena
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
        
        // Metodo que inicia el estado de ataque cuando está en estado "Running" y el player está cerca
        private void CheckPlayerPosition()
        {
            if (StateMachine.CurrentState == StateMachine.GaltzRunningState && _rotated)
            {
                if (Vector3.Distance(gameObject.transform.position, playerGameObject.transform.position) < 2)
                {
                    StateMachine.TransitionTo(StateMachine.GaltzAttackState);
                }
            }
        }
        
        // Metodo que comprueba hacia donde está yendo y hace girar y lo hace girar
        public void CheckDirection()
        {
            if (FacingRight && target.x < transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }
            
            if (!FacingRight && target.x > transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }
        }

        // Corutina de rotación
        private IEnumerator Rotate()
        {
            _rotated = false;
            CancelInvoke(nameof(TurnAround));
            InvokeRepeating(nameof(TurnAround),0f, 0.1f);
            yield return new WaitUntil(() => _rotated);
            CancelInvoke(nameof(TurnAround));
        }
        
        // Metodo que ejecuta la rotación
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

        // Metodo para elegir el escondite
        public void PlaceToHide(GameObject where)
        {
            if (where is null)
            {
                var placeToHide = Random.Range(0, hideouts.Length);
                if (placeToHide < 0 || placeToHide >= hideouts.Length) return;
                currentHideout = hideouts[placeToHide];
                target = hideouts[placeToHide].transform.position;
            }
            else
            {
                currentHideout = where;
                target = where.transform.position;
            }
            
        }

        // Metodo para recoger la posición del player
        public void FollowPlayer()
        {
            target = playerGameObject.transform.position;
        }

        // Metodo que hace moverse al galtzagorri
        public void Move()
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed);
        }
        
        // Metodo para que cuando el galtzagorri acaba el salto inicie el estado de "Hiding"
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Floor")) return;
            if (StateMachine.CurrentState == StateMachine.GaltzAttackState)
            {
                StateMachine.TransitionTo(StateMachine.GaltzHidingState);
            }
        }

        // Metodo para esperar 2 segundos y que el player esté disponible para salir del escondite
        public IEnumerator Wait()
        {
            waiting = true;
            yield return new WaitForSeconds(2f);
            waiting = false;
            yield return new WaitUntil(() => canExit && isIn);
            if(isIn) StateMachine.TransitionTo(StateMachine.GaltzRunningState);
        }
        
        // Metodo para cuando recibe daño
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        {
            base.OnReceiveDamage(attack, FacingRight);
            StartCoroutine(nameof(CoInvulnerability));
        }
        
        // Metodo que hace parpadear el sprite cuando recibe daño
        private IEnumerator CoInvulnerability()
        {
            _spriteRenderer.material.EnableKeyword("HITEFFECT_ON");
            while (Invulnerable)
            {
                _spriteRenderer.material.SetFloat(Alpha, 0.3f);
                                
                yield return new WaitForSeconds(0.02f);
                _spriteRenderer.material.SetFloat(Alpha, 1f);
                yield return new WaitForSeconds(0.05f);
            }
            _spriteRenderer.material.SetFloat(Alpha, 1f);
            _spriteRenderer.material.DisableKeyword("HITEFFECT_ON");
            yield return null;
        }

        // Metodo de muerte que inicia el estado "Death"
        public override void OnDeath()
        {
            StateMachine.TransitionTo(StateMachine.GaltzDeathState);
        }

        // Metodo con la lógica de la muerte
        public void Die()
        {
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            GaltzHideoutRange.PlayerEntered -= PlayerEntered;
            GaltzHideoutRange.PlayerEntered -= PlayerEntered;
            GaltzActiveZone.PlayerEnteredArea -= PlayerEnteredArea;
            GaltzActiveZone.PlayerExitedArea -= PlayerExitedArea;
            Invoke(nameof(DestroyThis), currentAnim.length + 2f);
        }
        
        // Método para destruir el galtzagorri cuando muere
        private void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}
