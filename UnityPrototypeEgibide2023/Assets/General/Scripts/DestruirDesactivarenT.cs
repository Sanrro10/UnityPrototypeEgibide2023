using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestruirDesactivarenT : MonoBehaviour
{
    [Header("Tiempo de existencia")]
    public float tiempoVida;
    [Header("Destruccion/Desactivacion")]
    [Tooltip("Si no true el objeto se desactiva")]
    public bool destruir;
    private void OnEnable()
    {
        Invoke("Destruccion", tiempoVida);
        Debug.Log("Activado");
    }
    private void Destruccion()
    {
        if (destruir)
        {
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

}
