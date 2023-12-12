using UnityEngine;

namespace Entities.Enemies.Witch.Scripts
{
    [CreateAssetMenu(fileName = "LandWitchData" , menuName = "LandWitchData", order = 5)]
    public class LandWitchData : ScriptableObject
    {
        public int health;
        public float missileSpeed;
        public int missileDamage;
        public float missileCooldown;
        public float magicCircleCooldown;
        public int magicCircleDamage;
        public float magicCircleChargeDuration;
        public float magicCircleActivationDelay;
        public float magicCircleEffectDuration;
        public float normalTeleportationCooldown;
        public float fastTeleportationCooldown;
    }
}
