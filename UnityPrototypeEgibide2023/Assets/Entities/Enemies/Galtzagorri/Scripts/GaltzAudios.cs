using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaltzAudios : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Audios _audios;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Audio1Play()
    {
        _audioSource.clip = _audios.audios[0];
        _audioSource.Play();
    }

    public void Audio2Play()
    {
        _audioSource.clip = _audios.audios[1];
        _audioSource.Play();
    }

    public void Audio3Play()
    {
        _audioSource.clip = _audios.audios[2];
        _audioSource.Play();
    }

    public void Audio4Play()
    {
        _audioSource.clip = _audios.audios[3];
        _audioSource.Play();
    }
}
