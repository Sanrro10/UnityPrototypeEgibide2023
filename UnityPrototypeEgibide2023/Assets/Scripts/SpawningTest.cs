using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningTest : MonoBehaviour
{
    public GameObject enemyType;
    private Vector3 _spawnPos = new Vector3(14.0500002f, 5.78000021f, 0f);
    
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
            Instantiate(enemyType, _spawnPos, Quaternion.identity);   
    }
}
