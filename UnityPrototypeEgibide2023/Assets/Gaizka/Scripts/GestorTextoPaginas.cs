using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GestorTextoPaginas : MonoBehaviour
{
    [SerializeField] private Book book;
    [SerializeField] private int page;
    public UnityEvent eventoP2;
    public UnityEvent eventoP4;
    public UnityEvent eventoP0;
    public OneSpriteFadeOnT [] textos;
    private OneSpriteFadeOnT actual;
    private void Start()
    {
        ComprobarPag();
    }

    public void ComprobarPag()
    {
        page = book.currentPage;
        AccionesEnPagina(page);
        Debug.Log ("Comprobar pagina");

    }

    void AccionesEnPagina(int pagina)
    {
        switch (pagina)
        {
            case 0:
                eventoP0.Invoke();
                break;
            case 2:
                eventoP2.Invoke();
                //textos[0].StartFadeIn();
                actual = textos[0];
                Debug.Log ("Segunda pagina");
                break;
            case 4:
                eventoP4.Invoke();
                actual = textos[1];
                print ("Cuarta pagina");
                break;  
            case 6:
                print ("Sexta pagina");
                break;
            default:
                //textos[0].StartFadeOut();
                print ("Incorrect intelligence level.");
                break;
        }
    }

    public void OcultarActual()
    {
        if (actual!=null)
        {
            actual.StartFadeOut(0.00001f);
        }
    }
}