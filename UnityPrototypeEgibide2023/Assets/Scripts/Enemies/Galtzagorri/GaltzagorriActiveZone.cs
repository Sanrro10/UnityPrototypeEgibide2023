using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GaltzagorriActiveZone : MonoBehaviour
{
    private GameObject _parent;

    void Start()
    {
        _parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var script = _parent.GetComponent<Galtzagorri>();
        script.ActivateEnemy();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        var script = _parent.GetComponent<Galtzagorri>();
        script.DeactivateEnemy();
    }
}
