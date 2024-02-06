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
    public void StartFadeOut()
    {
        fadeAmount = mat.GetFloat("_FadeAmount");
        StartCoroutine("FadeOut");
    }
    private IEnumerator FadeOut()
    {

        while (fadeAmount<1f)
        {
            mat.SetFloat("_FadeAmount", fadeAmount);
            yield return new WaitForSeconds(tiempo);
            fadeAmount += resta;
        }
        //return null;

    }
    public void StartFadeIn()
    {
        fadeAmount = mat.GetFloat("_FadeAmount");
        StartCoroutine("FadeIn");
    }
    private IEnumerator FadeIn()
    {

        while (fadeAmount>=0f)
        {
            mat.SetFloat("_FadeAmount", fadeAmount);
            yield return new WaitForSeconds(tiempo);
            fadeAmount -= resta;
        }
        //return null;

    }
  
}
