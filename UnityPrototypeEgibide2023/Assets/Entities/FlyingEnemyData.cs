using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "FlyingEnemyData", menuName = "FlyingEnemyData",order = 4)]
    public class FlyingEnemyData : ScriptableObject
    {
        public int health;
    }
}
