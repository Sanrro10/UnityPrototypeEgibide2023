using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OneSpriteFadeOnT : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float fadeAmount;
    [SerializeField] float tiempo;
    [SerializeField] float resta;
    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        mat = textMeshPro.fontMaterial;
        fadeAmount = mat.GetFloat("_FadeAmount");
    }
    public void StartFadeOut(float speed)
    {
        tiempo = speed;
        fadeAmount = mat.GetFloat("_FadeAmount");
        StartCoroutine("FadeOut",tiempo);
    }
    private IEnumerator FadeOut(float speed)
    {

        while (fadeAmount<1f)
        {
            mat.SetFloat("_FadeAmount", fadeAmount);
            yield return new WaitForSeconds(speed);
            fadeAmount += resta;
        }
        //return null;

    }
    public void StartFadeIn(float speed)
    {
        tiempo = speed;
        fadeAmount = mat.GetFloat("_FadeAmount");
        StartCoroutine("FadeIn", tiempo);
    }
    private IEnumerator FadeIn(float speed)
    {

        while (fadeAmount>=0f)
        {
            mat.SetFloat("_FadeAmount", fadeAmount);
            yield return new WaitForSeconds(speed);
            fadeAmount -= resta;
        }
        //return null;

    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
