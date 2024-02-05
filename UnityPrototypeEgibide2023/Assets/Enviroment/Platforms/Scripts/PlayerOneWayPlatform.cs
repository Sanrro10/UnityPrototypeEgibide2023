using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{

    private Collider2D _collider;
    private GameObject _target;
    private bool _canCollide = false;
    private void Start()
    {
        _target = GameObject.FindWithTag ("Player");
        _collider = GetComponent<Collider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_target == null)
        {
            _target = GameObject.FindWithTag("Player");
        }

        if (other.CompareTag("PlayerHelper") && other.name == "Feet" && _target.GetComponent<Rigidbody2D>().velocity.x <= 0f)
        {
            Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), false);
            _canCollide = true;
            return;
        }

        if (_canCollide) return;
        Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("PlayerHelper") && other.name == "Feet" && _target.GetComponent<Rigidbody2D>().velocity.y > 0f)
        {
            Physics2D.IgnoreCollision(_collider, _target.GetComponent<Collider2D>(), true);
            _canCollide = false;
            return;
        }
    }
}
