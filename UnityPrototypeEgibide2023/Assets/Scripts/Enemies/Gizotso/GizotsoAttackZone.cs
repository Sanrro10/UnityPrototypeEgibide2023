using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizotsoAttackZone : MonoBehaviour
{

    public void Attack()
    {
        StartCoroutine(nameof(Cooldown));
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        GetComponentInParent<PlayerController>().ReceiveDamage(1);
    }
}
