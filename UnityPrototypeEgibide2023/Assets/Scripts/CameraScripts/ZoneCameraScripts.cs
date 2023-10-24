using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ZoneCameraScripts : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    // Start is called before the first frame update

    private void Awake()
    {
        vcam = GetComponentInChildren<CinemachineVirtualCamera>(); 
        vcam.gameObject.SetActive(false);
    }

    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        vcam.gameObject.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        vcam.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }
}
