using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderAudios : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Audios _audios;
    private int time = 0;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void AudioThunderPlay()
    {
        _audioSource.clip = _audios.audios[0];
        _audioSource.Play();
    }
    public void AudioGlassPlay()
    {
        _audioSource.clip = _audios.audios[1];
        _audioSource.Play();
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
