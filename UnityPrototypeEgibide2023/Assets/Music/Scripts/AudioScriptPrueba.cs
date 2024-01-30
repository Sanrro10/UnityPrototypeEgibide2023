using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


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
        private GameObject ml;
        private Slider sl;
        private void Awake()
        {
            if (audioScript == null)
            {
                audioScript = this;
                //MusicSliderVolume = 
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
            //go = GameObject.Find("CanvasOptions");
            //Debug.Log(go.name);
            //value = audioVolume;
            //SetVolume(audioVolume);
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        
        public void SetVolume(Slider slider, float _value)
        {
            if (_value < 1)
            {
                _value = .001f;
            }
            
            RefreshVolume(slider, _value);

            if (slider.name == "MusicVolumeSlider")
            {
                PlayerPrefs.SetFloat("SavedWorldMusic", _value);
                masterMixer.SetFloat("WorldMusic", Mathf.Log10(_value / 100) * 20);
            }
            else if (slider.name == "EffectVolumeSlider")
            {
                PlayerPrefs.SetFloat("SavedEffectMusic", _value);
                masterMixer.SetFloat("SoundEffects", Mathf.Log10(_value / 100) * 20);
            } else if (slider.name == "MasterVolumeSlider")
            {
                PlayerPrefs.SetFloat("SavedMaster", _value);
                masterMixer.SetFloat("Master", Mathf.Log10(_value / 100) * 20);
            }
            
            //masterMixer.SetFloat("WorldMusic", Mathf.Log10(_value / 100) * 20);
        }

        public void ChangeMusicVolume()
        {
            SetVolume(MusicSliderVolume, MusicSliderVolume.value);
        }

        public void ChangeEffectVolume()
        {
            SetVolume(SoundEffectSliderVolume, SoundEffectSliderVolume.value);
        }

        public void ChangeMasterVolume()
        {
            SetVolume(masterSliderVolume, masterSliderVolume.value);
        }
        
        public void RefreshVolume(Slider slider, float _value)
        {
            slider.value = _value;
        }
        
    }
}