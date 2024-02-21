using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.Scripts;
using General.Scripts;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    private Vector3 _checkpoint;
    [SerializeField] ParticleSystem particles;
    private AudioSource audioFX;
    [SerializeField] float coolDownT;
    private bool enfriado = true;

    // Start is called before the first frame update
    void Start()
    {
        _checkpoint = transform.position;
        audioFX = GetComponentInChildren<AudioSource>();
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Colision con el player
        if (collision.CompareTag("Player"))
        {
             
            GameController.Instance.GetComponent<GameController>().SetCheckpoint(_checkpoint);
            collision.gameObject.GetComponent<PlayerController>().heal();
            if (enfriado)
            {
                particles.Play();
                audioFX.Play();
                enfriado = false;

                StartCoroutine(CoolDown());
            }
        }
    }

    private IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(coolDownT);
        enfriado = true;

    }
}
