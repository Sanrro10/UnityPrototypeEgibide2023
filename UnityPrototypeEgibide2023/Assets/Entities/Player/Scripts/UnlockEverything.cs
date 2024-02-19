using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.Scripts;
using UnityEngine;

public class UnlockEverything : MonoBehaviour
{
    // Start is called before the first frame update
    private InputActions _controls;
    public GameObject explosion;
    public GameObject thunder;

    public static event Action<GameObject[]> OnAllUnlock;
    void Start()
    {
#if UNITY_EDITOR
        _controls = new InputActions();
        _controls.Enable();
        _controls.GeneralActionMap.UnlockPotionExplosion.performed += ctx => OnAllUnlock?.Invoke(new []{explosion, thunder
    });
#endif
    }

    }

