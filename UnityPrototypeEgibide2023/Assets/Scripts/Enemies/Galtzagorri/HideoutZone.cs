using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideoutZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject parent = transform.parent.gameObject;
        var script = parent.GetComponent<Galtzagorri>();
        if (other.gameObject.CompareTag("Player")) {other.GetComponentInParent<PlayerController>().ReceiveDamage(1);}
        if (other.gameObject.CompareTag("EnemySpawnPoint")) script.ActivateHiding(other);
    }
}
