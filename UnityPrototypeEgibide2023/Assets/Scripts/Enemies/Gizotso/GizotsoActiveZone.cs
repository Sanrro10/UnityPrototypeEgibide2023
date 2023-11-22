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
        if (!other.gameObject.CompareTag("Player")) return;
        var script = _parent.GetComponent<Gizotso>();
        script.Attack();
    }
}
