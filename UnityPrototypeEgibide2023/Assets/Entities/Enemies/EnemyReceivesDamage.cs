using UnityEngine;

namespace Entities.Enemies
{
    public class EnemyReceivesDamage : MonoBehaviour
    {
        private int _currentHealth;
    
    
        [SerializeField] private PassiveEnemyData passiveEnemyData;
        // Start is called before the first frame update
        void Start()
        {
            _currentHealth = passiveEnemyData.health;
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        /*This makes the enemy receive damage, one damage, always, the PC will not have more than 1 damage*/
        public void ReceiveDamage()
        {
            _currentHealth--;
            if (_currentHealth == 0)
            {
                Death();
            }
        }

        private void Death()
        {
            Destroy(gameObject);
        }
    }
}