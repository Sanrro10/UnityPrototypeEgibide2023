using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Enemies.Galtzagorri.Scripts;
using Entities.Player.Scripts;
using General.Scripts;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

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
        private bool _goingRight;
    
        // Referencia al Animator
        [SerializeField] private Animator animator;
        
        // Referencia al objeto hijo de la hitbox de ataque
        [SerializeField] private GameObject attackHitBox;

        // Variable para controlar la velocidad
        [SerializeField] private float speed = 0.05f;
        
        // Variable que controla si está rotado
        private bool _rotated;
        
        // Variable que indica dónde se va a mover
        private Vector2 _target;

        private bool isDying;
        
        // Pasar nombre de booleanos a "IDs" para ahorrarnos comparaciones de strings
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsPreAttack = Animator.StringToHash("IsPreAttack");
        private static readonly int IsFirstAttack = Animator.StringToHash("IsFirstAttack");
        private static readonly int IsSecondAttack = Animator.StringToHash("IsSecondAttack");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        void Start()
        {
            // Añadir vida
            Health.Set(passiveEnemyData.health);
            
            // Coger las coordenadas de su límite derecho
            var rightLimit = gameObject.transform.Find("RightLimit");
            _rightLimitPosition = rightLimit.position;

            // Coger las coordenadas de su límite izquierdo
            var leftLimit = gameObject.transform.Find("LeftLimit");
            _leftLimitPosition = leftLimit.position;

            // Establecer que su target inicial sea el izquierdo
            _target = _leftLimitPosition;
        
            // Empezar el comportamiento
            InvokeRepeating(nameof(PassiveBehavior), 0f, 0.3f);
            InvokeRepeating(nameof(CheckDirection), 0f, 0.03f);
            InvokeRepeating(nameof(Move), 0f, 0.01f);
        }

        private void Move()
        {
            // Mover el gisotzo al target
            transform.position = Vector2.MoveTowards(transform.position, _target, speed);
        }

        private void CheckDirection()
        {
            if (_goingRight && _target.x < transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }

            if (!_goingRight && _target.x > transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }
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
            animator.SetBool(IsDead, false);
            animator.SetBool(IsIdle, true);

            // Comprobar si ha llegado al límite izquierdo
            if (Math.Abs(transform.position.x - _leftLimitPosition.x) < 0.5)
            {
                _onCooldown = true;
                _target = _rightLimitPosition;
                return;
            }
            
            // Comprobar si ha llegado al límite derecho
            if (Math.Abs(transform.position.x - _rightLimitPosition.x) < 0.5)
            {
                _onCooldown = true;
                _target = _leftLimitPosition;
            }
        }

        // Pequeña corutina para que mientres gire no pueda atacar al jugador
        private IEnumerator Rotate()
        {
            //_audioSource.clip = audioData.audios[1];
            //_audioSource.Play();
            _rotated = false;
            CancelInvoke(nameof(TurnAround));
            InvokeRepeating(nameof(TurnAround),0f, 0.1f);
            yield return new WaitUntil(() => _rotated);
            _onCooldown = false;
            CancelInvoke(nameof(TurnAround));
        }

        // Metodo que hace que se de la vuelta
        private void TurnAround()
        {
            int newEulerY;
            if (_goingRight)
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
                _goingRight = !_goingRight;
            } 
        }

        public void Attack()
        {
            // Emprezar corutina de ataque si no está atacando ya
            if (!_attacking && !_onCooldown && !isDying)
            {
                CancelInvoke(nameof(PassiveBehavior));
                CancelInvoke(nameof(Move));
                StartCoroutine(nameof(Cooldown));
            }
        }

        private IEnumerator Cooldown()
        {
            // Control de estados
            _attacking = true;
            _onCooldown = true;
            
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
            
            // Hacer que se mueva un poco durante la animación de ataque si el jugador se a alejado
            var script = GetComponentInChildren<GizotsoActiveZone>();
            if (!script.PlayerInside)
            {
                InvokeRepeating(nameof(Dash),0f,0.05f);
                yield return new WaitForSeconds(lengthAnim);
                CancelInvoke(nameof(Dash));
            }
            else
            {
                yield return new WaitForSeconds(lengthAnim);
            }
            
            // Desactivar hitbox de ataque
            attackHitBox.SetActive(false);
            
            /* SEGUNDO GOLPE (igual que el primero) */
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, true);
            
            currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            lengthAnim = currentAnim.length;
            
            attackHitBox.SetActive(true);
            
            if (!script.PlayerInside)
            {
                InvokeRepeating(nameof(Dash),0f,0.05f);
                yield return new WaitForSeconds(lengthAnim);
                CancelInvoke(nameof(Dash));
            }
            else
            {
                yield return new WaitForSeconds(lengthAnim);
            }
            
            // Desactivar hitbox de ataque
            attackHitBox.SetActive(false);
            
            // Animacion de stun
            animator.SetBool(IsSecondAttack, false);
            //animator.SetBool(IsHurt, true);
            
            // TODO: Tiempo que va a estar stuneado
            yield return new WaitForSeconds(1.5f);
            
            //animator.SetBool(IsHurt, false);
            animator.SetBool(IsIdle, true);
            
            _attacking = false;
            
            // Tiempo hasta que puede hacer otro ataque para que no se quede en bucle atacando
            //yield return new WaitForSeconds(1f);
            _onCooldown = false;
            
            InvokeRepeating(nameof(PassiveBehavior), 0f, 0.1f);
            InvokeRepeating(nameof(Move), 0f, 0.01f);
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
            isDying = true;
            
            StopAllCoroutines();
            CancelInvoke();
            
            // Animación muerte
            animator.SetBool(IsPreAttack, false);
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, false);
            animator.SetBool(IsHurt,false);
            animator.SetBool(IsIdle, false);
            animator.SetBool(IsDead, true);

            Rigidbody2D component = GetComponent<Rigidbody2D>();
            component.isKinematic = true;
            component.simulated = false;
            
            PolygonCollider2D gisotzoCollider = gameObject.GetComponent<PolygonCollider2D>();
            gisotzoCollider.enabled = false;
            
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            float lengthAnim = currentAnim.length;
            Invoke(nameof(DestroyThis),lengthAnim + 2f);
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
