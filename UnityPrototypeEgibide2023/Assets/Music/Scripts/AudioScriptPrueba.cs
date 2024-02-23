using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

namespace DigitalRuby.SoundManagerNamespace
{
    public class AudioScriptPrueba : MonoBehaviour
    {
        public static AudioScriptPrueba audioScript;
        [SerializeField] private Slider MusicSliderVolume;
        [SerializeField] private Audios scriptableWorldAudios;
        [SerializeField] private Slider SoundEffectSliderVolume;
        
        [SerializeField] private Slider masterSliderVolume;
        private float value;
        [SerializeField] private AudioMixer masterMixer;

        private GameObject go;
        private bool isOk;
        private Slider sl;

        [SerializeField] AudioSource musicSource;
        [SerializeField] AudioClip musica1;
        [SerializeField] AudioClip musica2;
        public float fadeDuration = 1.0f;

        private void Awake()
        {
            if (audioScript == null)
            {
                audioScript = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        
        // Start is called before the first frame update
        void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            //value = audioVolume;
            //SetVolume(audioVolume);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
        {
            Debug.Log("la escena cargada tiene un indice =" + scene.buildIndex);
            switch (scene.buildIndex)
            {
                case 1:
                    audioScript.StartCoroutine(PlayMusicWithFade(musica1));
                    break;
                case 13:
                    audioScript.StartCoroutine(PlayMusicWithFade(musica2));
                    break;
                // Agrega m�s casos seg�n sea necesario para tus escenas
                default:
                    //StartCoroutine(PlayDefaultMusicWithFade());
                    break;
            }
        }
        private IEnumerator PlayMusicWithFade(AudioClip music)
        {
            float initialVolume = 0.0f;
            float targetVolume = 1.0f;
            float fadeTimer = 0.0f;

            audioScript.musicSource.Stop();
            audioScript.musicSource.clip = music;
            audioScript.musicSource.Play();

            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                float t = fadeTimer / fadeDuration;
                audioScript.musicSource.volume = Mathf.Lerp(initialVolume, targetVolume, t);
                yield return null;
            }

            audioScript.musicSource.volume = targetVolume;
        }
        // Update is called once per frame
        void Update()
        {
            //Debug.Log(go.name);
        }
        
        public void SetVolume(Slider slider, float _value)
        {
            if (_value < 1)
            {
                _value = .001f;
            }
            
            audioScript.RefreshVolume(slider, _value);

            if (slider.name == "MusicVolumeSlider")
            {
                PlayerPrefs.SetFloat("SavedWorldMusic", _value);
                audioScript.masterMixer.SetFloat("WorldMusic", Mathf.Log10(_value / 100) * 20);
            }
            else if (slider.name == "EffectVolumeSlider")
            {
                PlayerPrefs.SetFloat("SavedEffectMusic", _value);
                audioScript.masterMixer.SetFloat("SoundEffects", Mathf.Log10(_value / 100) * 20);
            } else if (slider.name == "MasterVolumeSlider")
            {
                PlayerPrefs.SetFloat("SavedMaster", _value);
                audioScript.masterMixer.SetFloat("Master", Mathf.Log10(_value / 100) * 20);
            }
            
            //masterMixer.SetFloat("WorldMusic", Mathf.Log10(_value / 100) * 20);
        }

        public void ChangeMusicVolume()
        {
            audioScript.SetVolume(MusicSliderVolume, MusicSliderVolume.value);
        }

        public void ChangeEffectVolume()
        {
            audioScript.SetVolume(SoundEffectSliderVolume, SoundEffectSliderVolume.value);
        }

        public void ChangeMasterVolume()
        {
            audioScript.SetVolume(masterSliderVolume, masterSliderVolume.value);
        }
        
        public void RefreshVolume(Slider slider, float _value)
        {
            slider.value = _value;
        }
        
    }
}