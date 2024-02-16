using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialTexts : MonoBehaviour
{
    [SerializeField] private Texture control;
    [SerializeField] private String linea1;
    [SerializeField] private String linea2;
    [SerializeField] private GameObject line1;
    [SerializeField] private GameObject line2;
    [SerializeField] private GameObject image;
    
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
            if (control is not null)
            {
                image.SetActive(true);
            }
            image.GetComponent<RawImage>().texture = control;
            line1.GetComponent<TextMeshProUGUI>().text = linea1;
            line2.GetComponent<TextMeshProUGUI>().text = linea2;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (control is not null)
            {
                image.SetActive(false);
            }
            line1.GetComponent<TextMeshProUGUI>().text = "";
            line2.GetComponent<TextMeshProUGUI>().text = "";
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
