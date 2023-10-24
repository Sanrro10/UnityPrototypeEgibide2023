using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    private int _healthPoints;
    private EntityControler controler;

    private void Start()
    {
        controler = GetComponent<EntityControler>();
    }
    
    public void Set(int points)
    {
        _healthPoints = points;
    }

    public int Get()
    {
        return _healthPoints;
    }

    public void AddHealth(int points)
    {
        _healthPoints = _healthPoints + points;
    }
    
    public void RemoveHealth(int points)
    {
        if (_healthPoints <= 0)
        {
           controler.OnDeath();
        }
        else
        {
            _healthPoints = _healthPoints - points;
        }
    }    
}
