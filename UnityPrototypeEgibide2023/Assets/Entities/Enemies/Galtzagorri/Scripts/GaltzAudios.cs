using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaltzAudios : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Audios _audios;
    int time;
    
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

    public void AudioIdleOutPlay()
    {
        //_audioSource.clip = _audios.audios[1];
        _audioSource.PlayOneShot(_audios.audios[1]);
    }

    public void AudioIdlePlay()
    {
        //_audioSource.clip = _audios.audios[2];
        if (time == 0)
        {
            _audioSource.PlayOneShot(_audios.audios[2]);
            time = 5;
        }
        else
        {
            time -= 1;
        }
        
        
    }

    public void AudioDeadPlay()
    {
        //_audioSource.clip = _audios.audios[3];
        
        _audioSource.PlayOneShot(_audios.audios[3]);
    }

    public void AudioHurtPlay()
    {
        _audioSource.clip = _audios.audios[4];
        _audioSource.Play();
    }
}
