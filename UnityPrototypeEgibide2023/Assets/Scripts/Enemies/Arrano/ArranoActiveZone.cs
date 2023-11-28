using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArranoActiveZone : MonoBehaviour
{
    private GameObject _parent;
    public bool inside;
    private Collider2D _playerCollider;

    void Start()
    {
        _parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _playerCollider.GetComponentInParent<PlayerController>().ReceiveDamage(1);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        inside = false;
    }

    public void Hit()
    {
        
    }
}
