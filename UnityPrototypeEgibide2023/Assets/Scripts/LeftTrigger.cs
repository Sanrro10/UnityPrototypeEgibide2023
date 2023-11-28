using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftTrigger : MonoBehaviour
{
    private BoxCollider2D _collider2D;
    private int _numberOfGrounds = 0;
    [SerializeField] private PlayerController _playerController;
    // Start is called before the first frame update
    void Start()
    {
        _collider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor") || other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            _numberOfGrounds++;
            _playerController.isCollidingLeft = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Floor") || other.CompareTag("Wall") || other.CompareTag("Enemy"))
        {
            _numberOfGrounds--;
            if (_numberOfGrounds == 0)  _playerController.isCollidingLeft = false;
        }    
    }
}
