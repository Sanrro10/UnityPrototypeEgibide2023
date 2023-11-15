using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private List <GameObject> _gameObjects = new List<GameObject>();
    
    private void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Enemy" && !EnemyAllreadyDamage(collider2D.gameObject))
        {
            collider2D.gameObject.GetComponent<HealthComponent>().RemoveHealth(1);
            _gameObjects.Add(collider2D.gameObject);
        }
    }
    
    private void OnDisable()
    {
        _gameObjects.Clear();
    }
    
    private bool EnemyAllreadyDamage(GameObject gameObject)
    {
        bool allreadyDamage = false;
        for (int i = 0; i < _gameObjects.Count; i++)
        {
            if (_gameObjects[i].Equals(gameObject))
            {
                allreadyDamage = true;
            }
        }
        return allreadyDamage;
    }
}

