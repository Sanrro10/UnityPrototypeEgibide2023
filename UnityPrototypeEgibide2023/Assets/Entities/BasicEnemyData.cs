using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "BasicEnemyData", menuName = "BasicEnemyData", order = 3)]
    public class BasicEnemyData : ScriptableObject
    {
        public int health;
    }
}
