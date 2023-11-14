using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideoutZone : MonoBehaviour
{
    private bool _enemyHit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject parent = transform.parent.gameObject;
        var script = parent.GetComponent<Galtzagorri>();
        if (other.gameObject.CompareTag("Player") && !_enemyHit) {
            other.GetComponentInParent<PlayerController>().ReceiveDamage(1);
            _enemyHit = true;
        }
        if (other.gameObject.CompareTag("EnemySpawnPoint")) script.ActivateHiding(other);
    }

    public void ResetHit()
    {
        _enemyHit = false;
    }
}
