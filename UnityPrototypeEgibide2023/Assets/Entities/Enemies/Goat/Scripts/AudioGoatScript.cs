using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioGoatScript : MonoBehaviour
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
    
    public void PlayIdleAudio()
    {
        _audioSource.clip = _audios.audios[1];
        _audioSource.Play();
    }

    public void PlayAttackAudio()
    {
        _audioSource.clip = _audios.audios[0];
        _audioSource.Play();
    }

    public void PlayPrepareAttackAudio()
    {
        _audioSource.clip = _audios.audios[2];
        _audioSource.Play();
    }

    public void PlayChargeAttackAudio()
    {
        _audioSource.clip = _audios.audios[3];
        _audioSource.Play();
    }

    public void PlayStunnedAudio()
    {
        _audioSource.clip = _audios.audios[4];
        _audioSource.Play();
    }
}
