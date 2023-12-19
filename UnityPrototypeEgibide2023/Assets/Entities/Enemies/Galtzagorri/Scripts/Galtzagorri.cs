using System.Collections;
using General.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class Galtzagorri : EntityControler
    {
        // Referencia del jugador
        private GameObject _playerGameObject;
    
        // Datos del Galtzagorri
        [SerializeField] private BasicEnemyData basicEnemyData;
    
        // Array de escondites
        [SerializeField] private GameObject[] hideouts;

        // Referencia del script para los escondites
        [SerializeField] private HideoutZone scriptHideout;
    
        // Referencia para que el Navmesh sea accesible desde todos lados
        private NavMeshAgent _navMeshAgent;
    
        // Variable para que espere X segundos al aparecer
        private bool _waiting;
    
        // Variable para que espere X segundos hasta irse a un escondite (por si el Player está saltando, etc)
        private bool _waitingForPlayer;
    
        // Variable para que vaya a la última posición conocida del jugador
        private Vector3 _lastAvailablePosition;
    
        // Variable que controla si está en proceso de esconderse
        private bool _hiding;
    
        // Variable que controla si está escondido
        private bool _hidden;
    
        // Variable que controla si setá atacando
        private bool _attacking;

        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        
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
                if (_hiding || _hidden) return;
                _navMeshAgent.SetDestination(_lastAvailablePosition);
                if (!_waitingForPlayer)
                {
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
            if (_hiding || _hidden) yield break;
            Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
            _attacking = true;
            CancelInvoke(nameof(ChasePlayer));
            _navMeshAgent.enabled = false;
            
            // TODO: Animacion Ataque
            
            
            rb2D.velocity = new Vector2(0, 0);
            rb2D.AddForce(new Vector2((_playerGameObject.transform.position.x - transform.position.x) * 2, 5), ForceMode2D.Impulse);
            yield return new WaitForSeconds(1f);
            rb2D.velocity = new Vector2(0, 0);
            _navMeshAgent.enabled = true;
            ActivateEnemy();
            Hide();
            yield return new WaitForSeconds(1f);
            _attacking = false;

        }

        // Metodo que comprueba si el Player es accesible
        private bool CanReachPlayer()
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
    
        // Metodo que activa el meterse al escondite
        public void ActivateHiding(Collider2D other)
        {
            if (Vector3.Distance(other.gameObject.transform.position, _navMeshAgent.destination) < 1 && _hiding)
            {
                // TODO: Animacion Idle
                _navMeshAgent.isStopped = true;
                Rigidbody2D rb2D = GetComponent<Rigidbody2D>();
                rb2D.velocity = new Vector2(0, 0);
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
            scriptHideout.ResetHit();
        }

        // Metodo que desactiva el enemigo
        public void DeactivateEnemy()
        {
            CancelInvoke(nameof(ChasePlayer));
            //Hide();
        }
    
        public override void OnDeath()
        {
            // TODO Activar animacion de muerte y logica relacionada
        
            Invoke(nameof(DestroyThis),2f);
        
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}
