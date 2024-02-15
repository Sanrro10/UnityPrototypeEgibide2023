using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnEnableUISelect : MonoBehaviour
{
   [SerializeField] Button botonSelected;

    private void OnEnable()
    {
        botonSelected.Select();
    }
}
