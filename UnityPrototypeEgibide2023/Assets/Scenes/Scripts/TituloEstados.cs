using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TituloEstados : MonoBehaviour
{
    [SerializeField] private Animator estadoAnimator;
    // Start is called before the first frame update
    public void Estado1()
    {
        estadoAnimator.SetTrigger("APaso2");
    }
    public void Estado2()
    {
        estadoAnimator.SetTrigger("APaso3");
    }
    public void Estado3()
    {
        estadoAnimator.SetTrigger("APaso1");
    }
}
