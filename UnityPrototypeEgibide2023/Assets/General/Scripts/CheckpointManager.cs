using System.Collections;
using System.Collections.Generic;
using General.Scripts;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
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
            GameController.Instance.GetComponent<GameController>().SetCheckpoint(_checkpoint);
        }
    }
}
