using System;
using UnityEngine;

namespace Entities.Destructible
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class DestructibleController : EntityControler
    {
        [SerializeField] private AttackComponent.AttackType vulnerabilityType = AttackComponent.AttackType.Normal;
        [SerializeField] private int numberOfHitsToDestroy = 1;
        
        [SerializeField] GameObject explotion;
        private bool asExploded = false;
        private GameObject spawnedExplotion;

        private Animator _animator;
        private static readonly int FailedHit = Animator.StringToHash("FailedHit");
        private static readonly int Hit = Animator.StringToHash("Hit");
        private static readonly int Remove = Animator.StringToHash("Remove");

        // Start is called before the first frame update
        void Start()
        {
            Health.Set(numberOfHitsToDestroy);
        }
    
        public override void OnReceiveDamage(AttackComponent.AttackData attack, bool toTheRight = true)
        {
            Debug.Log(attack);
            if (attack.attackType != vulnerabilityType)
            {
                return;
            }
            Debug.Log("Damage");
            Health.RemoveHealth(1);
        }
    
        public override void OnDeath()
        {
            //Instanciate explotion
            asExploded = true;
            spawnedExplotion = Instantiate(explotion, transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z), Quaternion.identity);
            gameObject.SetActive(false);
            Invoke("DeleteRock", 1);
            
            
            //Destroy(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            
            Debug.Log("Collision" + other.gameObject.name);
        }
        
        private void DeleteRock()
        { 
            Destroy(gameObject);
        
        }
    }
}
