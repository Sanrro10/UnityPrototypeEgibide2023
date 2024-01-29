using System.Collections.Generic;
using UnityEngine;

namespace Music.Scripts
{
    public class AudioScript : MonoBehaviour
    {
        public static AudioScript audioScript;
        private AudioSource _audioSource;
        //[SerializeField]private AudioClip[] _clip;
        [SerializeField] private List<AudioClip> clips = new List<AudioClip>();

        private void Awake()
        {
            if (audioScript == null)
            {
                audioScript = this;
                DontDestroyOnLoad(transform.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            Canciones();
            _audioSource.clip = clips[0];
            _audioSource.Play();

        
            if (!_audioSource.isPlaying)
            {
            
            }
        
            if (true)
            {
            
            }

        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void Canciones()
        {
            // Hay que crear una carpeta en Assets y colocar todos los objetos ahi si se quiere
            // usar Resources.Load
            clips.Add(Resources.Load<AudioClip>("HITMARKER SOUND EFFECT"));
            clips.Add(Resources.Load<AudioClip>("Canciones/bakemonogatari Ost 05 Hanekawa Tsubasa no Baai"));
            clips.Add(Resources.Load<AudioClip>("Canciones/16 - Last Dinosaur"));
            clips.Add(Resources.Load<AudioClip>("Canciones/Heavy Day - Guilty Gear Xrd SIGN"));
        }

        public void hitPlayer()
        {
            _audioSource.PlayOneShot(clips[0]);
        }
    
        public void RespawnPlayer()
        {
            _audioSource.PlayOneShot(clips[2]);
        }

        // public void DeadPlayer()
        // {
        //     _audioSource.PlayOneShot(clips[2]);
        // }
    
    
    }
}
