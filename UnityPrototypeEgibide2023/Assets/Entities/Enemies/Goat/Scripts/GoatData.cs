using UnityEngine;

namespace Entities.Enemies.Goat.Scripts
{
    [CreateAssetMenu(fileName = "GoatData", menuName = "ScriptableObjects", order = 1)]
    public class GoatData : ScriptableObject
    {
        public float movementSpeed;
        public float force;
        public float stunTime;
        public float waitTime;
        public float jumpForce;
        public float visionDistance;
        public int health;
    }
}
