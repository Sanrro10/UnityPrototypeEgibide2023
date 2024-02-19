using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudios : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Audios _audios;
    private int time = 0;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void AudioWalkPlay()
    {
        if (time <= 0)
        {
            _audioSource.clip = _audios.audios[0];
            //_audioSource.volume = 0f;
            _audioSource.Play();
            time = 2;
        }
        else
        {
            time -= 1;
        }
    }
    
    public void AudioFallDamagePlay()
    {
        _audioSource.clip = _audios.audios[1];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }

    public void AudioAttacksPlay()
    {
        _audioSource.clip = _audios.audios[2];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }

    public void AudioDashPlay()
    {
        _audioSource.clip = _audios.audios[2];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }
    public void AudioHurtPlay()
    {
        _audioSource.clip = _audios.audios[5];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }
    
    public void AudioDeadPlay()
    {
        _audioSource.clip = _audios.audios[6];
        _audioSource.volume = 0.5f;
        _audioSource.Play();
    }

    public void AudioRunPlay()
    {
        _audioSource.clip = _audios.audios[3];
        _audioSource.Play();
    }

    public void AudioJumpPlay()
    {
        _audioSource.clip = _audios.audios[3];
        _audioSource.Play();
    }

    public void AudioThrowPotionPlay()
    {
        _audioSource.clip = _audios.audios[4];
        _audioSource.Play();
    }
    
    
    
    
    
    
    // Update is called once per frame
    void Update()
    {
        
    }
    
    
}
