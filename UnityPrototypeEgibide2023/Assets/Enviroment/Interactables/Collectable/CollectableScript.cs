using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.Scripts;
using UnityEngine;

public class CollectableScript : MonoBehaviour
{
    [SerializeField] private PlayerData _playerData;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerData.collectables++;
            Destroy(this.gameObject);
        }
    }
    
}
