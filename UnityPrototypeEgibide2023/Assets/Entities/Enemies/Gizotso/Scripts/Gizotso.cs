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

        [SerializeField] private Animator gizotsoAnimator;
        // Variable para controlar la direccion
        private bool _goingRight = true;
        //AttackZone cacheado
        [SerializeField] private float fuerzaSalto;
        [SerializeField] private Rigidbody2D gizoRigidBody;
        [SerializeField] private GizotsoAttackZone scriptaAttackZone;
        [SerializeField] private GameObject attackZone;
        // Variable para acceder al Navmesh desde todos lados
        private NavMeshAgent _navMeshAgent;
        //duracion animaciones
        [SerializeField] private float duracionClipActual;
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
            gizotsoAnimator.SetBool("andando", true);
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
            gizotsoAnimator.SetBool("andando", false);

            attacking = true;
            gizotsoAnimator.SetBool("atacando", true);
            // TODO: Animacion Pre-Ataque
            yield return new WaitForSeconds(1f);

            // TODO: Animacion Ataque
            
        
            // TODO --------------------------------------------------------------------------
            // CUADRAR TIEMPOS CON LA ANIMACION
        
            // Primer golpe 
            attackZone.SetActive(true);
            gizotsoAnimator.SetTrigger("primerGolpe");
            if (gizotsoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                // Obtener el clip de animación actual
                AnimationClip clip = gizotsoAnimator.GetCurrentAnimatorClipInfo(0)[0].clip;

                // Obtener la duración del clip de animación
                 duracionClipActual = clip.length;

                // Hacer algo con la duración (por ejemplo, imprimir en la consola)
                Debug.Log("Duración de la animación actual: " + duracionClipActual);
            }
            InvokeRepeating(nameof(Dash), duracionClipActual, 0.05f);
            scriptaAttackZone.Attack();
            yield return new WaitForSeconds(0.5f);
            CancelInvoke(nameof(Dash));
        
            // Segundo golpe
            attackZone.SetActive(true);
            gizotsoAnimator.SetTrigger("segundoGolpe");
            if (gizotsoAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                // Obtener el clip de animación actual
                AnimationClip clip = gizotsoAnimator.GetCurrentAnimatorClipInfo(0)[0].clip;

                // Obtener la duración del clip de animación
                 duracionClipActual = clip.length;

                // Hacer algo con la duración (por ejemplo, imprimir en la consola)
                Debug.LogError("Duración de la animación actual: " + duracionClipActual);
            }
            InvokeRepeating(nameof(Dash), duracionClipActual, 0.05f);
            scriptaAttackZone.Attack();
            yield return new WaitForSeconds(0.5f);
            CancelInvoke(nameof(Dash));
            _navMeshAgent.enabled = true;
        
            // TODO: Animacion de stun
            // TODO: Tiempo que va a estar stuneado
            yield return new WaitForSeconds(2f);
        
            // TODO: Animacion idle
        
            _navMeshAgent.isStopped = false;
            gizotsoAnimator.SetBool("idle", true);
            gizotsoAnimator.SetBool("atacando", false);
            // Tiempo hasta que puede hacer otro ataque para que no se quede en bucle atacando
            yield return new WaitForSeconds(3f);
            _navMeshAgent.enabled = true;
            attacking = false;
        }

        private void Dash()
        {
            Debug.Log("Dash llamado");
            gizotsoAnimator.SetTrigger("dash");
            _navMeshAgent.enabled = false;
            if (_goingRight)
            {
               // gizoRigidBody.AddForce(new Vector2(transform.position.x*fuerzaSalto,transform.position.y* fuerzaSalto), ForceMode2D.Impulse);
                //gizoRigidBody.AddForce(Vector2.up* fuerzaSalto, ForceMode2D.Impulse);
                
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 2f, transform.position.y+2f), fuerzaSalto);
            }
            else
            {
                //gizoRigidBody.AddForce(new Vector2(transform.position.x * -fuerzaSalto, transform.position.y * fuerzaSalto), ForceMode2D.Force);

                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 2f, transform.position.y+2f), fuerzaSalto);
            }
        }
    
        public override void OnDeath()
        {
            // TODO Animacion Muerte
            gizotsoAnimator.SetTrigger("muerto");
            Invoke(nameof(DestroyThis),2f);
        
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }

    }
}
