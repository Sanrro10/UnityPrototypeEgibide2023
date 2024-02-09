using System;
using System.Collections;
using Entities.Enemies.Galtzagorri.Scripts.StatePattern;
using General.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Enemies.Galtzagorri.Scripts
{
    public class NewGaltzScript : EntityControler
    {
        [SerializeField] private float speed = 0.05f;
        [SerializeField] public GameObject[] hideouts;
        [SerializeField] private BasicEnemyData data;
        [SerializeField] public GameObject attackZone;
        [SerializeField] public GameObject startHideout;
        [SerializeField] public GameObject activeZone;
        
        public bool waiting;
        public GaltzStateMachine StateMachine;
        public Animator animator;
        public GameObject playerGameObject;
        public Vector2 target;
        public GameObject currentHideout;
        
        private SpriteRenderer _spriteRenderer;
        private bool _rotated = true;
        public bool canExit = true;
        public bool isIn;
        
        private static readonly int Alpha = Shader.PropertyToID("_Alpha");

        private void Start()
        {
            StateMachine = new GaltzStateMachine(this);
            StateMachine.Initialize(StateMachine.GaltzHidingState);
            PlaceToHide(startHideout);
            Health.Set(data.health);
            
            GaltzHideoutRange.PlayerEntered += PlayerEntered;
            GaltzHideoutRange.PlayerExited += PlayerExited;
            GaltzActiveZone.PlayerEnteredArea += PlayerEnteredArea;
            GaltzActiveZone.PlayerExitedArea += PlayerExitedArea;
            
            playerGameObject = GameController.Instance.GetPlayerGameObject();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            InvokeRepeating(nameof(CheckDirection), 0f, 0.03f);
            InvokeRepeating(nameof(CheckPlayerPosition), 0f, 0.01f);
        }

        private void PlayerEntered(GameObject hideout)
        {
            if (hideout.Equals(currentHideout))
            {
                canExit = false;
            }
        }

        private void PlayerExited(GameObject hideout)
        {
            if (hideout.Equals(currentHideout))
            {
                canExit = true;
            }
        }

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
                if (Vector3.Distance(gameObject.transform.position, playerGameObject.transform.position) < 2)
                {
                    StateMachine.TransitionTo(StateMachine.GaltzAttackState);
                }
            }
        }
        
        public void CheckDirection()
        {
            //if (StateMachine.CurrentState == StateMachine.GaltzHiddenState) return;
            
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

        public void FollowPlayer()
        {
            target = playerGameObject.transform.position;
        }

        public void Move()
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.x, transform.position.y), speed);
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
            waiting = true;
            yield return new WaitForSeconds(2f);
            waiting = false;
            yield return new WaitUntil(() => canExit && isIn);
            if(isIn) StateMachine.TransitionTo(StateMachine.GaltzRunningState);
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
                _spriteRenderer.material.SetFloat(Alpha, 0.3f);
                                
                yield return new WaitForSeconds(0.02f);
                _spriteRenderer.material.SetFloat(Alpha, 1f);
                yield return new WaitForSeconds(0.05f);
            }
            _spriteRenderer.material.SetFloat(Alpha, 1f);
            _spriteRenderer.material.DisableKeyword("HITEFFECT_ON");
            yield return null;
        }

        public override void OnDeath()
        {
            StateMachine.TransitionTo(StateMachine.GaltzDeathState);
        }

        public void Die()
        {
            AnimationClip currentAnim = animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            GaltzHideoutRange.PlayerEntered -= PlayerEntered;
            GaltzHideoutRange.PlayerEntered -= PlayerEntered;
            GaltzActiveZone.PlayerEnteredArea -= PlayerEnteredArea;
            GaltzActiveZone.PlayerExitedArea -= PlayerExitedArea;
            Invoke(nameof(DestroyThis), currentAnim.length + 2f);
        }
        
        private void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}
