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
        // Referencia al audio source
        [SerializeField] private AudioSource _audioSource;
        
        // Datos del enemigo
        [SerializeField] private PassiveEnemyData passiveEnemyData;
        
        // Referencia al sprite renderer
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        // Variable para que no ande ni entre en bucle de ataques mientras está atacando
        private bool _attacking;
        
        // Variable para añadir cooldown al ataque
        private bool _onCooldown;
        
        // Variable para controlar la animacion del ataque
        private bool _attackAnim;
    
        // Referencia al Animator
        [SerializeField] private Animator animator;
        
        // Referencia a los audios
        [SerializeField] private Audios audioData;
        
        // Referencia al objeto hijo de la hitbox de ataque
        [SerializeField] private GameObject attackHitBox;

        // Variable para controlar la velocidad
        [SerializeField] private float speed = 0.05f;
        
        // Variable que controla si está rotado
        private bool _rotated;
        
        // Variable que indica dónde se va a mover
        private Vector2 _target;

        // Variable que controla el estado de muerte
        private bool _isDying;
        
        private int _tiempoTotal = 30;
        private int _tiempoAudioIdle = 0;
        private int _tiempoAudioAttack = 0;
        private int _tiempoAudioUp = 0;
        
        // Pasar nombre de booleanos a "IDs" para ahorrarnos comparaciones de strings
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");
        private static readonly int IsPreAttack = Animator.StringToHash("IsPreAttack");
        private static readonly int IsFirstAttack = Animator.StringToHash("IsFirstAttack");
        private static readonly int IsSecondAttack = Animator.StringToHash("IsSecondAttack");
        private static readonly int IsHurt = Animator.StringToHash("IsHurt");
        private static readonly int IsDead = Animator.StringToHash("IsDead");

        void Start()
        {
            attackHitBox.SetActive(false);
            
            // Añadir vida
            Health.Set(passiveEnemyData.health);

            // Establecer que su target inicial sea el izquierdo
            _target = new Vector2(transform.position.x - 1000, transform.position.y);

            _audioSource = GetComponent<AudioSource>();
            
            animator.SetBool(IsPreAttack, false);
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, false);
            animator.SetBool(IsHurt,false);
            animator.SetBool(IsDead, false);
            animator.SetBool(IsIdle, true);
            
            // Empezar el comportamiento
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
            // Hace girar el gisotzo cuando está yendo a la derecha pero el nuevo target está a la izquierda
            if (FacingRight && _target.x < transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
            }

            // Hace girar el gisotzo cuando está yendo a la izquierda pero el nuevo target está a la derecha
            if (!FacingRight && _target.x > transform.position.x)
            {
                StartCoroutine(nameof(Rotate));
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
                _onCooldown = false;
            } 
        }

        public void PrepareAttack()
        {
            // Emprezar corutina de ataque si no está atacando ya
            if (!_attacking && !_onCooldown && !_isDying)
            {
                CancelInvoke(nameof(Move));
                StartCoroutine(nameof(Attack));
            }
        }

        private IEnumerator Attack()
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
            
            // Animacion primer ataque
            animator.SetBool(IsPreAttack, false);
            animator.SetBool(IsFirstAttack, true);

            // Primer golpe
            _attackAnim = false;
            StartCoroutine(nameof(AttackAnim));
            yield return new WaitUntil(() => _attackAnim);
            
            // Animación segundo ataque
            animator.SetBool(IsFirstAttack, false);
            animator.SetBool(IsSecondAttack, true);

            // Segundo golpe
            _attackAnim = false;
            StartCoroutine(nameof(AttackAnim));
            yield return new WaitUntil(() => _attackAnim);
            
            // Animacion de stun
            animator.SetBool(IsSecondAttack, false);
            //animator.SetBool(IsHurt, true);
            
            // TODO: Tiempo que va a estar stuneado
            yield return new WaitForSeconds(0.2f);
            
            //animator.SetBool(IsHurt, false);
            animator.SetBool(IsIdle, true);
            
            _attacking = false;
            _onCooldown = false;
            
            InvokeRepeating(nameof(Move), 0f, 0.01f);
        }

        private IEnumerator AttackAnim()
        {
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            float lengthAnim = currentAnim.length;
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
            _attackAnim = true;
            
            // Desactivar hitbox de ataque
            attackHitBox.SetActive(false);
        }

        // Metodo que mueve al enemigo durante la animación del ataque
        private void Dash()
        {
            if (FacingRight)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 0.1f, transform.position.y), 0.1f);
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 0.1f, transform.position.y), 0.1f);
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
            _isDying = true;
            
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
            
            PolygonCollider2D gisotzoCollider = gameObject.GetComponentInChildren<PolygonCollider2D>();
            gisotzoCollider.enabled = false;
            
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            float lengthAnim = currentAnim.length;
            Invoke(nameof(DestroyThis),lengthAnim + 2f);
        }
        
        private void DestroyThis()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("GisotzoRotatePoint")) return;
            _onCooldown = true;
            _target = FacingRight ? new Vector2(transform.position.x - 1000, transform.position.y) : new Vector2(transform.position.x + 1000, transform.position.y);
        }

        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        {
            base.OnReceiveDamage(attack, FacingRight);
            StartCoroutine(nameof(CoInvulnerability));
        }
        
        private IEnumerator CoInvulnerability()
        {
            spriteRenderer.material.EnableKeyword("HITEFFECT_ON");
            while (Invulnerable)
            {
                spriteRenderer.material.SetFloat("_Alpha", 0.3f);
                                
                yield return new WaitForSeconds(0.02f);
                spriteRenderer.material.SetFloat("_Alpha", 1f);
                yield return new WaitForSeconds(0.05f);
            }
            spriteRenderer.material.SetFloat("_Alpha", 1f);
            spriteRenderer.material.DisableKeyword("HITEFFECT_ON");
            yield return null;
        }
    }
}
