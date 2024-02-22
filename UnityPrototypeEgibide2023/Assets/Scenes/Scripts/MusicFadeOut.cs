using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicFadeOut : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] float fadeDuration = 1f;

    public void StartFadeOut()
 {
        StartCoroutine("PlayMusicWithFade");
}

    private IEnumerator PlayMusicWithFade()
    {
        float initialVolume = 0.0f;
        float targetVolume = 1.0f;
        float fadeTimer = 0.0f;


        while (fadeTimer < fadeDuration)
        {
            fadeTimer += Time.deltaTime;
            float t = fadeTimer / fadeDuration;
            musicSource.volume = Mathf.Lerp(initialVolume, targetVolume, t);
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}
