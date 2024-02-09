using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class TutorialTexts : MonoBehaviour
{
    [SerializeField] private GameObject tutorialActual;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private Animator animator;
    [SerializeField] private Texture2D XButton;
    [SerializeField] private Sprite YButton;
    [SerializeField] private Sprite AButton;
    [SerializeField] private Sprite BButton;
    [SerializeField] private Sprite LBButton;
    [SerializeField] private Sprite RBButton;
    private Color colorText = Color.white;
    private TextMeshProUGUI texto;
    private float fadeTime = 2.5f;
    private string moverte = "[W/S] []\nPara moverte";
    private string saltar = "[SPACE] [<sprite=2>    ]\npara saltar";
    private string dash = "[SHIFT] [<sprite=3>    ]\npara hacer dash e ir rapido";
    private string checkpoint = "Si mueres reapareceras en el checkpoint";
    private string atacar = "[J/Click izquierdo] [<sprite=0>    ]\nAtacar al enemigo";
    private string LanzarPocion = "[F] [<sprite=1>    ]\nLanza la pocion\n\nGolpea la pocion en el aire para lanzarla mas lejos";
    private string textoAMostrar = "";
    // Start is called before the first frame update
    void Start()
    {
        // ParaMoverse = GameObject.Find("[W/S]ParaMoverte");
        // tutorial = GameObject.Find("Tutorial");
        // ParaSaltar = GameObject.Find("[Space]ParaSaltar");
        // ParaHacerDash = GameObject.Find("[Shift]ParaHacerDashIrRapido");
        // ParaCheckpoint = GameObject.Find("SiMueresReaparecesEnElCheckpoint");
        // ParaAtacar = GameObject.Find("[J/ClickIzquierdo]AtacarAlEnemigo");
        // ParaLanzarPocion = GameObject.Find("[F]LanzarLaPocion");
        //texto = tutorialUI.gameObject.GetComponent<TextMeshProUGUI>();
        texto = tutorialUI.gameObject.GetComponent<TextMeshProUGUI>();
        // XButton = GetComponent<Image>();
        // YButton = 
        // AButton = 
        // BButton = 
        // LBButton = 
        // RBButton = 
        //
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (tutorialActual.name)
            {
                case "[W/S]ParaMoverte":
                    textoAMostrar = moverte;
                    break;
                case "[Space]ParaSaltar":
                    textoAMostrar = saltar;
                    break;
                case "[Shift]ParaHacerDashIrRapido":
                    textoAMostrar = dash;
                    break;
                case "SiMueresReaparecesEnElCheckpoint":
                    textoAMostrar = checkpoint;
                    break;
                case "[J/ClickIzquierdo]AtacarAlEnemigo":
                    textoAMostrar = atacar;
                    break;
                case "[F]LanzarLaPocion":
                    textoAMostrar = LanzarPocion;
                    break;
            }
            //TODO: Difuminar textos con una corrutina
            // animator.SetBool("EntriAnim", true);
            // animator.SetBool("EndAnim", false);
            StartCoroutine(FadeInAnim());
            texto.text = textoAMostrar;
            StopCoroutine(FadeInAnim());
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // animator.SetBool("EntriAnim", false);
            // animator.SetBool("EndAnim", true);
            StartCoroutine(FadeOutAnim());
            StartCoroutine(StayText());
            StopCoroutine(StayText());
            StopCoroutine(FadeOutAnim());
        }
    }

    public IEnumerator FadeInAnim()
    {
        colorText = new Color(colorText.r, colorText.g, colorText.b, 0);
            while (texto.color.a < 1.0f)
            {
                texto.color = new Color(texto.color.r, texto.color.g, texto.color.b,
                    texto.color.a + (Time.deltaTime * fadeTime));
                yield return null;
            }
            
    }

    public IEnumerator FadeOutAnim()
    {
        colorText = new Color(colorText.r, colorText.g, colorText.b, 1);
        while (texto.color.a > 0.0f)
        {
            texto.color = new Color(texto.color.r, texto.color.g, texto.color.b,
                texto.color.a - (Time.deltaTime * fadeTime));
            yield return null;
        }

    }

    public IEnumerator StayText()
    {
        yield return new WaitForSeconds(1f);
        texto.text = "";
    }


    // public void OnTriggerStay2D(Collider2D other)
    // {
    //     tutorialUI.gameObject.GetComponent<TextMeshProUGUI>().text = textoAMostrar;
    // }


    /*    ParaMoverse = GameObject.Find("[W/S]ParaMoverte").GetComponentInChildren<TextMeshPro>();
    ParaSaltar = GameObject.Find("[Space]ParaSaltar").GetComponentInChildren<TextMeshPro>();
    ParaHacerDash = GameObject.Find("[Shift]ParaHacerDashIrRapido").GetComponentInChildren<TextMeshPro>();
    ParaCheckpoint = GameObject.Find("SiMueresReaparecesEnElCheckpoint").GetComponentInChildren<TextMeshPro>();
    ParaAtacar = GameObject.Find("[J/ClickIzquierdo]AtacarAlEnemigo").GetComponentInChildren<TextMeshPro>();
    ParaLanzarPocion = GameObject.Find("[F]LanzarLaPocion").GetComponentInChildren<TextMeshPro>();*/
    

    // Update is called once per frame
    void Update()
    {
        
    }
}
