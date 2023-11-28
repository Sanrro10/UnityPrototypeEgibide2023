using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizotsoActiveZone : MonoBehaviour
{
    private GameObject _parent;

    void Start()
    {
        _parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        if (!other.gameObject.CompareTag("Player")) return;
        Debug.Log("B");
        var script = _parent.GetComponent<Gizotso>();
        script.Attack();
    }
}
