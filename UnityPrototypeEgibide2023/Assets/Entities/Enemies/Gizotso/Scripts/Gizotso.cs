using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies.Gizotso.Scripts
{
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
    
        // Referencia al Animator
        [SerializeField] private Animator _animator;
        
        private static readonly int andando = Animator.StringToHash("andando");
        private static readonly int muerto = Animator.StringToHash("muerto");
        private static readonly int atacando = Animator.StringToHash("atacando");
        private static readonly int primerGolpe = Animator.StringToHash("primerGolpe");
        private static readonly int segundoGolpe = Animator.StringToHash("segundoGolpe");
        private static readonly int dash = Animator.StringToHash("dash");


        void Start()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();

            _animator = GetComponentInChildren<Animator>();
            
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
            if (attacking) return;
            
            // TODO: Animacion Walk
            _animator.SetBool(muerto, false);
            _animator.SetBool(atacando, false);
            _animator.SetBool(primerGolpe, false);
            _animator.SetBool(segundoGolpe, false);
            _animator.SetBool(dash, false);
            _animator.SetBool(andando, true);
            
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
            _animator.SetBool(atacando, true);
            
            yield return new WaitForSeconds(1f);
        
            
            
            GameObject attackZone = gameObject.transform.Find("AttackZone").gameObject;
        
            // TODO --------------------------------------------------------------------------
            // CUADRAR TIEMPOS CON LA ANIMACION
            
            // Primer golpe
            // TODO: Animacion Ataque
            _animator.SetBool(atacando, false);
            _animator.SetBool(primerGolpe, true);
            attackZone.SetActive(true);
            InvokeRepeating(nameof(Dash),0f,0.05f);
            GetComponentInChildren<GizotsoAttackZone>().Attack();
            yield return new WaitForSeconds(0.5f);
            CancelInvoke(nameof(Dash));
        
            // Segundo golpe
            // TODO: Animacion Ataque
            _animator.SetBool(primerGolpe, false);
            _animator.SetBool(segundoGolpe, true);
            attackZone.SetActive(true);
            InvokeRepeating(nameof(Dash),0f,0.05f);
            GetComponentInChildren<GizotsoAttackZone>().Attack();
            yield return new WaitForSeconds(0.5f);
            CancelInvoke(nameof(Dash));
        
            // TODO: Animacion de stun
            _animator.SetBool(segundoGolpe, false);
            _animator.SetBool(dash, true);
            
            // TODO: Tiempo que va a estar stuneado
            yield return new WaitForSeconds(2f);
        
            _navMeshAgent.isStopped = false;
            _animator.SetBool(dash, false);
            _animator.SetBool(andando, true);
            
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
}
