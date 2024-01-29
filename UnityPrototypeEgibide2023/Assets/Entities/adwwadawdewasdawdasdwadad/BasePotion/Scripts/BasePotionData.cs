using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    [CreateAssetMenu(fileName = "BasePotionData", menuName = "BasePotionData", order = 1)]
    public class BasePotionData : ScriptableObject
    {
        public int health;
        public int damage;
        public int speed;
        public GameObject explosion;
    }
}