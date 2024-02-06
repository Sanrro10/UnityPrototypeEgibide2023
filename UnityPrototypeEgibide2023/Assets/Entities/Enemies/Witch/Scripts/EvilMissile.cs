using System.Collections;
using Entities.Player.Scripts;
using General.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace Entities.Enemies.Witch.Scripts
{
    public class EvilMissile : MonoBehaviour
    {
        [SerializeField] private GameObject explosion;
        [SerializeField] private Animator missileAnimator;
        
        private Rigidbody2D _missileBody;
        private GameObject _playerRef;
        public LandWitchData witchData;
        private bool _damageDealt = false;
        [SerializeField] private AttackComponent _attackComponent;

        private float _angle;

        void Start()
        {
            //_attackComponent = GetComponentInChildren<AttackComponent>();
            _attackComponent.ActivateHitbox();
            //witchData = GameObject.Find("SorginaLand").GetComponent<LandWitch>().landWitchData;
            _playerRef = GameController.Instance.GetPlayerGameObject();
            _missileBody = gameObject.GetComponent<Rigidbody2D>();
            _attackComponent.AddAttackData(new AttackComponent.AttackData(witchData.missileDamage, 0, new Vector2(0,0), 6, AttackComponent.AttackType.Normal));
            Rotacion();
            StartCoroutine(nameof(ApplyForce),0f);
            //Maximun Time Alive
            Invoke(nameof(Delete),15f);
        
        
        }

        /*Rotates the proyectile so that its X rotation var points to the player*/
        private void Rotacion()
        {
            var neededRotation = Quaternion.LookRotation(_playerRef.transform.position - transform.position);
            transform.rotation = neededRotation;
        }
    
        /*The missile hits something, if it is the player, damage them, else destroy de missile*/
        void OnCollisionEnter2D(Collision2D collision)
        {
            Explode();
        }

        /*Launches the Missile Towards the player*/
        private IEnumerator ApplyForce()
        {
            yield return new WaitForSeconds(0.5f);
            Rotacion();
            Vector2 whereToGoPlease = _playerRef.transform.position - transform.position;
            whereToGoPlease.Normalize();
            Vector2 speedwagon = whereToGoPlease * witchData.missileSpeed;
            _missileBody.velocity = speedwagon;
        }
    

        private void Explode()
        {   
            _attackComponent.DeactivateHitbox();
            //Instantiate(explosion, transform.position, Quaternion.identity);
            missileAnimator.SetTrigger("MissileExplode");
            _missileBody.constraints = RigidbodyConstraints2D.FreezeAll;
            Invoke(nameof(Delete), 0.05f);
        }

        private void Delete()
        {
            Instantiate(explosion, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y), Quaternion.identity);
            Destroy(gameObject);
        }
        

    }
}
