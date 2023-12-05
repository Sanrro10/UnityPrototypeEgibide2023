using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerOneWayPlatform : MonoBehaviour
{

    private GameObject _currentOneWayPlatform;

    [SerializeField] private CapsuleCollider2D playerCollider;

    [SerializeField] private CapsuleCollider2D excaliburCollider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _currentOneWayPlatform = other.gameObject;
        StartCoroutine(nameof(DisableCollision));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OneWayPlatform"))
        {
            _currentOneWayPlatform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = _currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        Physics2D.IgnoreCollision(excaliburCollider, platformCollider);
        yield return new WaitForSeconds(1.5f);
        Physics2D.IgnoreCollision(playerCollider,platformCollider, false);
        Physics2D.IgnoreCollision(excaliburCollider,platformCollider, false);
    }
}
