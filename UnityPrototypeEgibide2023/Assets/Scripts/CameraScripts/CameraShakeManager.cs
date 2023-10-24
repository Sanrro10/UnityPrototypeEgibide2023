using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    // [SerializeField] private Pla
    public static CameraShakeManager instance;

    [SerializeField] private float globalShake = 1f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShake);
    }
}
