using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaltzAudios : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Audios _audios;
    private int tiempoIdle = 0;
    private int tiempoHide = 0;
    private int tiempoAttack = 0;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AudioAttackPlay()
    {
        if (tiempoAttack <= 0)
        {
            _audioSource.clip = _audios.audios[0];
            _audioSource.Play();
            tiempoAttack = 3;
        }
        else
        {
            tiempoHide -= 1;
            tiempoIdle -= 1;
            tiempoAttack -= 1;
        }
        
        
    }

    public void AudioHidePlay()
    {
        if (tiempoHide <= 0)
        {
            _audioSource.clip = _audios.audios[1];
            _audioSource.Play();
            tiempoHide = 3;
        }
        else
        {
            tiempoHide -= 1;
            tiempoIdle -= 1;
            tiempoAttack -= 1;
        }
        
    }

    public void AudioIdleInPlay()
    {
        if (tiempoIdle <= 0)
        {
            _audioSource.clip = _audios.audios[2]; 
            _audioSource.Play();
            tiempoIdle = 4;
        }
        else
        {
            tiempoHide -= 1;
            tiempoIdle -= 1;
            tiempoAttack -= 1;
        }
    }

    public void AudioDeadPlay()
    {
        _audioSource.clip = _audios.audios[3];
        _audioSource.Play();
    }

    public void AudioIdleOutPlay()
    {
        _audioSource.clip = _audios.audios[4];
        _audioSource.Play();
    }
}
