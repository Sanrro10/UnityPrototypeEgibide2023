using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShakeManager : MonoBehaviour
{
    // [SerializeField] private Pla
    public static CameraShakeManager instance;

    [SerializeField] private float globalShake = 0.10f;
    [SerializeField] private float impulseDuration = 0.15f;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.m_ImpulseDefinition.m_ImpulseDuration = impulseDuration;
        impulseSource.m_DefaultVelocity.y = 0.75f;
        impulseSource.GenerateImpulseWithForce(globalShake);
    }
}
