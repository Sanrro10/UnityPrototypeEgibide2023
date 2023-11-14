using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGroundedLogic : MonoBehaviour
{

    [SerializeField] private GameObject player;

    private PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
        pc = player.GetComponent<PlayerController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Floor") || collision.gameObject.tag.Equals("Enemy"))
        {
            pc.SetNumberOfGrounds(pc.GetNumberOfGrounds() + 1);
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag.Equals("Floor") || collision.gameObject.tag.Equals("Enemy"))
        {
            pc.SetNumberOfGrounds(pc.GetNumberOfGrounds() - 1);
        }
    }
}
