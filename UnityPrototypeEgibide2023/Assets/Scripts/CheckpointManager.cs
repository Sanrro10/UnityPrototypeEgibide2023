using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public GameObject gameControler;
    private Vector3 _checkpoint;
    
    // Start is called before the first frame update
    void Start()
    {
        _checkpoint = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Colision con el player
        if (collision.gameObject.tag == "Player")
        {
            gameControler.GetComponent<RespawnManager>().SetCheckpoint(_checkpoint);
        }
    }
}
