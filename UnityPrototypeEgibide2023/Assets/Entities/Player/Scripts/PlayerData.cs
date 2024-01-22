using UnityEngine;

namespace Entities.Player.Scripts
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "PlayerData", order = 1)]
    public class PlayerData : ScriptableObject
    {
        public float movementSpeed;
        public float jumpPower;
        public bool facingRight;
        public float downAttack;
        public float dashSpeed;
        public float dashCooldown;
        public float gravity;
        public float potionColdownTime;
        public float jumpDuration;
        public float floatDuration;
        public float airdashDuration;
        public float maxAirHorizontalSpeed;
        public float maxFallSpeed;
        public bool airDashUnlocked = false;
        public float groundDashDuration;
        public float collectables;

        public int damage;
        public float knockback;
        public float knockbackPotion;
        
        public AnimationCurve dashCurve;
        public float airdashForce;
    }
}

