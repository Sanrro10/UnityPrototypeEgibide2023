using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReceivesDamage : MonoBehaviour
{
    private int currentHealth;
    
    
    [SerializeField] private PassiveEnemyData _passiveEnemyData;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = _passiveEnemyData.health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*This makes the enemy receive damage, one damage, always, the PC will not have more than 1 damage*/
    public void receiveDamage()
    {
        currentHealth--;
        if (currentHealth == 0)
        {
            death();
        }
    }

    private void death()
    {
        Destroy(gameObject);
    }
}
