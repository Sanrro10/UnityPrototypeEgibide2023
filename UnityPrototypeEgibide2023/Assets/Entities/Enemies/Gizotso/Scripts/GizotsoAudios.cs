using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizotsoAudios : MonoBehaviour
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

    public void AudioIdlePlay()
    {
        _audioSource.clip = _audios.audios[0];
        _audioSource.Play();
    }

    public void AudioPreAttackPlay()
    {
        _audioSource.clip = _audios.audios[1];
        _audioSource.Play();
    }

    public void AudioFirstAttackPlay()
    {
        _audioSource.clip = _audios.audios[2];
        _audioSource.Play();
    }

    public void AudioSecondAttackPlay()
    {
        _audioSource.clip = _audios.audios[3];
        _audioSource.Play();
    }

    public void AudioHurtPlay()
    {
        _audioSource.clip = _audios.audios[4];
        _audioSource.Play();
    }

    public void AudioDeadPlay()
    {
        _audioSource.clip = _audios.audios[5];
        _audioSource.Play();
    }
}
