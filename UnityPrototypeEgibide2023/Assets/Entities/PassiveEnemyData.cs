using UnityEngine;

namespace Entities
{
    [CreateAssetMenu(fileName = "PassiveEnemyData", menuName = "PassiveEnemyData",order = 2)]
    public class PassiveEnemyData : ScriptableObject
    {
        public int health;
    }
}
