using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;

public class OneSpriteFadeOnT : MonoBehaviour
{
    [SerializeField] Material mat;
    [SerializeField] float fadeAmount;
    [SerializeField] float tiempo;
    [SerializeField] float resta;
    [SerializeField] float valorInicial;

    [SerializeField] bool sourceMat;
    public UnityEvent fadeCompletado;

    private void OnDisable()
    {
        if (valorInicial != null)
        {
            mat.SetFloat("_FadeAmount", valorInicial);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        if (sourceMat)
        {
            mat = textMeshPro.fontSharedMaterial;

        }
        else
        {
            mat = textMeshPro.fontMaterial;

        }
        fadeAmount = mat.GetFloat("_FadeAmount");
        if (valorInicial != null)
        {
            mat.SetFloat("_FadeAmount", valorInicial);
        }
    }
    public void StartFadeOut(float speed)
    {
        tiempo = speed;
        fadeAmount = mat.GetFloat("_FadeAmount");
        fadeAmount = 0f;
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
        //FadeCompletado();
    }

    private void FadeCompletado()
    {
        fadeCompletado.Invoke();
    }

    public void StartFadeIn(float speed)
    {
        tiempo = speed;
        fadeAmount = mat.GetFloat("_FadeAmount");
        fadeAmount = 1f;
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
        FadeCompletado();
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}
