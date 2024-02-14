using UnityEngine;

public class GizotsoAudios : MonoBehaviour
{
    private AudioSource _audioSource;
    [SerializeField] private Audios _audios;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
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
