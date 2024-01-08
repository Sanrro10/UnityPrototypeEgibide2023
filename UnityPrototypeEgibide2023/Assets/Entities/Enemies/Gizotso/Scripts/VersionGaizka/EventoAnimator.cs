using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoAnimator : MonoBehaviour
{
    [Header("Evento Cambio direccion rayo")]
    [SerializeField] BoxCollider2D colliderRayo;
    [SerializeField] float offsetRayoX;
    [SerializeField] Vector2 offsetRayo;
    [SerializeField] GizotsoControl gizoControl;
    [Header("Evento triger ataque")]
    [SerializeField] BoxCollider2D colliderAtaque;


    public void DireccionColliderRayo()
    {
        gizoControl.mirandoDerecha = !gizoControl.mirandoDerecha;
        offsetRayoX = gizoControl.origenRayo.localPosition.x;
        offsetRayoX *= -1;
        //Debug.LogError("OffsetRayoX multiplicado es = " + offsetRayoX);
        gizoControl.origenRayo.localPosition = new Vector3(offsetRayoX, gizoControl.origenRayo.localPosition.y, 0);
    }
    public void ColliderAtaque()
    {
        if (colliderAtaque.isActiveAndEnabled)
        {
            // Si está activo, desactívalo
            colliderAtaque.enabled= false;
        }
        else
        {
            // Si está inactivo, actívalo
            colliderAtaque.enabled = true;
        }
    }
}
