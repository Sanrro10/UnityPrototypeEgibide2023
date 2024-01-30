using Entities.Player.Scripts;
using General.Scripts;
using UnityEngine;

namespace Entities.Enemies.Witch.Scripts
{
    public class MagicCircle : MonoBehaviour
    {

        private int _damage;

        [SerializeField] private Animator magicCircleAnimator;
        [SerializeField] private SpriteRenderer magicCircleSprites;
        private GameObject _playerGoRef;
        private PlayerController _playerRef;
        private LandWitchData _witchData;
        [SerializeField] private AttackComponent _attackComponent;
        
        void Awake()
        {
        }
        // Start is called before the first frame update
        void Start()
        {
            _attackComponent = GetComponentInChildren<AttackComponent>(true);
            _attackComponent.DeactivateHitbox();
            _playerGoRef = GameController.Instance.GetPlayerGameObject();
            _playerRef = GameController.Instance.GetPlayerController();
            _witchData = GameObject.Find("SorginaLand").GetComponent<LandWitch>().landWitchData;
            _attackComponent.AddAttackData(new AttackComponent.AttackData(_witchData.magicCircleDamage, 0, new Vector2(0,0), 6, AttackComponent.AttackType.Projectile));


            InvokeRepeating(nameof(MovementLogic),0,0.02f);
            Invoke(nameof(ActivationLogic), _witchData.magicCircleChargeDuration);
        }

        // Update is called once per frame
        void Update()
        {
            //transform.localScale = Vector3.one * (1 + 0.2f * Mathf.Sin(Time.time));
        }

        private void MovementLogic()
        {
            //Might need changes
            gameObject.transform.position = _playerGoRef.transform.position;
        }

        private void ActivationLogic()
        {
            //ChangeColor();
            CancelInvoke(nameof(MovementLogic));
            Invoke(nameof(Activate),_witchData.magicCircleActivationDelay);
        }

        private void Activate()
        {
            //ChangeColor();
            //magicCircleAnimator.SetTrigger("MagicCircleActivate");
            Debug.Log("Magic Circle Activated");
            _attackComponent.ActivateHitbox();
            Invoke(nameof(EndOfLife), _witchData.magicCircleEffectDuration);
        }
        private void EndOfLife()
        {
            _attackComponent.DeactivateHitbox();
            Destroy(gameObject);
        }

        /*private void ChangeColor()
        {
            if (magicCircleSprites.color == Color.clear)
            {
                magicCircleSprites.color = Color.red;    
            }else if (magicCircleSprites.color == Color.red)
            {
                magicCircleSprites.color = Color.clear;
            }


        }*/
    }
}
