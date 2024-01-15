using UnityEngine;

namespace Entities.Potions.BasePotion.Scripts
{
    [CreateAssetMenu(fileName = "BasePotionData", menuName = "ScriptableObjects", order = 1)]
    public class BasePotionData : ScriptableObject
    {
        public int health;
        public int damage;
        public int speed;
        public GameObject explosion;
    }
}