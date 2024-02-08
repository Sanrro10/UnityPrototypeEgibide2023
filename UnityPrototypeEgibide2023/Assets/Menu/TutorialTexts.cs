using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialTexts : MonoBehaviour
{
    [SerializeField] private GameObject tutorialActual;
    [SerializeField] private GameObject tutorialUI;
    private string moverte = "[W/S]\nPara moverte";
    private string saltar = "[SPACE]\npara saltar";
    private string dash = "[SHIFT]\npara hacer dash e ir rapido";
    private string checkpoint = "Si mueres reapareceras en el checkpoint";
    private string atacar = "[J/Click izquierdo]\nAtacar al enemigo";
    private string LanzarPocion = "[F]\nLanza la pocion\n\nGolpea la pocion en el aire para lanzarla mas lejos";
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
            tutorialUI.gameObject.GetComponent<TextMeshProUGUI>().text = textoAMostrar;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tutorialUI.gameObject.GetComponent<TextMeshProUGUI>().text = "";
        }
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
