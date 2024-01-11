using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAnimEventHandler : MonoBehaviour
{

    [SerializeField] private AudioSource witchAudio;
    [SerializeField] private Audios landWitchAudios;

    private Animator _animator;
    //Start is called before the first frame update
    void Start()
    {
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WitchLaught()
    {
        _animator.SetInteger("WitchLaught", Random.Range(1, 11));
    }

    public void WitchStopLaught()
    {
        _animator.SetInteger("WitchLaught", 1);
    }

    public void WitchPlaySound(int clipAudio)
    {
        if (_animator.GetBool("WitchDeathDmg") == true)
        {
            witchAudio.clip = landWitchAudios.audios[7];
            witchAudio.Play();
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            return;
        }

        witchAudio.clip = landWitchAudios.audios[clipAudio];
        witchAudio.Play();
        
    }

    public void WitchPlaySoundLaught()
    {
        witchAudio.clip = landWitchAudios.audios[Random.Range(0, 2)];
        witchAudio.Play();
    }
}
