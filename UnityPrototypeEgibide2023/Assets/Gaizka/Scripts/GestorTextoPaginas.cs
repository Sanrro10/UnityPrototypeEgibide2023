using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GestorTextoPaginas : MonoBehaviour
{
    [SerializeField] private Book book;
    [SerializeField] private int page;
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
            case 2:
                textos[0].StartFadeIn();
                actual = textos[0];
                Debug.Log ("Why hello there good sir! Let me teach you about Trigonometry!");
                break;
            case 4:
                print ("Hello and good day!");
                break;
            case 6:
                print ("Whadya want?");
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
            actual.StartFadeOut();
        }
    }
}
