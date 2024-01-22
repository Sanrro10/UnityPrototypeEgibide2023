using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

namespace Entities.Enemies.Gizotso.Scripts
{
    public class Gizotso : EntityControler
    {
        // Datos del enemigo
        [SerializeField] private PassiveEnemyData passiveEnemyData;
        
        // Variable para que no ande ni entre en bucle de ataques mientras está atacando
        private bool _attacking;
        
        // Variable para añadir cooldown al ataque
        private bool _onCooldown;
    
        // Posiciones de los límites entre los que andas
        private Vector3 _leftLimitPosition;
        private Vector3 _rightLimitPosition;
    
        // Variable para controlar la direccion
        private bool _goingRight = true;
    
        // Variable para acceder al Navmesh desde todos lados
        private NavMeshAgent _navMeshAgent;
    
        // Referencia al Animator
        [SerializeField] private Animator animator;
        
        // Referencia al objeto hijo de la hitbox de ataque
        [SerializeField] private GameObject attackHitBox;
        
        // Pasar nombre de booleanos a "IDs" para ahorrarnos comparaciones de strings
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsPreAttack = Animator.StringToHash("IsPreAttack");
        private static readonly int IsFirstAttack = Animator.StringToHash("IsFirstAttack");
        private static readonly int IsSecondAttack = Animator.StringToHash("IsSecondAttack");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int IsDetah = Animator.StringToHash("IsDeath");

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
            // Si está atacando no se mueve
            if (_attacking) return;
            
            // Animacion Idle, setea el resto de aniamciones a False por si acaso
            animator.SetBool(IsPreAttack, false);
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, false);
            animator.SetBool(IsHurt,false);
            animator.SetBool(IsDetah, false);
            animator.SetBool(IsIdle, true);
            
            // Cambia de dirección
            _navMeshAgent.SetDestination(_goingRight ? _rightLimitPosition : _leftLimitPosition);
            
            // Comprobar si ha llegado al límite izquierdo
            if (Math.Abs(transform.position.x - _leftLimitPosition.x) < 0.5)
            {
                _onCooldown = true;
                _goingRight = true;
                StartCoroutine(nameof(Turn));
                return;
            }
            
            // Comprobar si ha llegado al límite derecho
            if (Math.Abs(transform.position.x - _rightLimitPosition.x) < 0.5)
            {
                _onCooldown = true;
                _goingRight = false;
                StartCoroutine(nameof(Turn));
            }
        }

        // Pequeña corutina para que mientres gire no pueda atacar al jugador
        private IEnumerator Turn()
        {
            yield return new WaitForSeconds(0.75f);
            _onCooldown = false;
        }

        public void Attack()
        {
            // Emprezar corutina de ataque si no está atacando ya
            if (!_attacking && !_onCooldown)
            {
                StartCoroutine(nameof(Cooldown));
            }
        }

        private IEnumerator Cooldown()
        {
            // Control de estados
            _attacking = true;
            _onCooldown = true;
            
            // Para el NavMesh para que deje de andar
            _navMeshAgent.isStopped = true;
            
            // Animacion Pre-Ataque
            animator.SetBool(IsIdle, false);
            animator.SetBool(IsPreAttack, true);
            
            // Coge la duración de la animación actual para que la ejecución no siga hasta que termine
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            float lengthAnim = currentAnim.length;
            yield return new WaitForSeconds(lengthAnim);
            
            // Animacion Ataque
            animator.SetBool(IsPreAttack, false);
            animator.SetBool(IsFirstAttack, true);
            
            currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            lengthAnim = currentAnim.length;
            
            /* PRIMER GOLPE */
            // Activar hitbox de ataque
            attackHitBox.SetActive(true);
            
            // Hacer que se mueva un poco durante la animación de ataque
            InvokeRepeating(nameof(Dash),0f,0.05f);
            yield return new WaitForSeconds(lengthAnim);
            CancelInvoke(nameof(Dash));
            
            // Desactivar hitbox de ataque
            attackHitBox.SetActive(false);
            
            /* SEGUNDO GOLPE (igual que el primero) */
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, true);
            
            currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            lengthAnim = currentAnim.length;
            
            attackHitBox.SetActive(true);
            InvokeRepeating(nameof(Dash),0f,0.05f);
            yield return new WaitForSeconds(lengthAnim);
            CancelInvoke(nameof(Dash));
            
            // Desactivar hitbox de ataque
            attackHitBox.SetActive(false);
            
            // Animacion de stun
            animator.SetBool(IsSecondAttack, false);
            //animator.SetBool(IsHurt, true);
            
            // TODO: Tiempo que va a estar stuneado
            yield return new WaitForSeconds(0.1f);
        
            // Volver a activar el NavMesh para que se mueve
            _navMeshAgent.isStopped = false;
            //animator.SetBool(IsHurt, false);
            animator.SetBool(IsIdle, true);
            
            _attacking = false;
            
            // Tiempo hasta que puede hacer otro ataque para que no se quede en bucle atacando
            yield return new WaitForSeconds(1f);
            _onCooldown = false;
        }

        // Metodo que mueve al enemigo durante la animación del ataque
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
            // Animación muerte
            animator.SetBool(IsPreAttack, false);
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, false);
            animator.SetBool(IsHurt,false);
            animator.SetBool(IsIdle, false);
            animator.SetBool(IsDetah, true);
        
            Invoke(nameof(DestroyThis),2f);
        }

        public override void OnReceiveDamage(int damage, float knockback, Vector2 angle, bool facingRight = true)
        {
            base.OnReceiveDamage(damage, knockback, angle);
            
            //TODO Incluir logica de recibir daño, si es que la tiene
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }

    }
}
