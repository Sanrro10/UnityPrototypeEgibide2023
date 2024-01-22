using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    [CreateAssetMenu(fileName = "GoatData", menuName = "ScriptableObjects", order = 1)]
    public class GoatData : ScriptableObject
    {
        public float movementSpeed;
        public float knockback;
        public int damage;
        public Vector2 angle;
        public float stunTime;
        public float waitTime;
        public float jumpForce;
        public float visionDistance;
        public int health;
    }
}
