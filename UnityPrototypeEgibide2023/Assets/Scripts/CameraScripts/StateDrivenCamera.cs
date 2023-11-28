using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class StateDrivenCamera : MonoBehaviour
{
    private CinemachineStateDrivenCamera _cinemachine;
   // private CinemachineVirtualCamera _virtual_camera;
    
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _cinemachine = GetComponent<CinemachineStateDrivenCamera>();
        // _virtual_camera = GetComponentInChildren<CinemachineVirtualCamera>();
        
        if (_player == null)
        {
            _player = GameController.Instance.GetPlayerGameObject();
            //_player = GameObject.Find("Player Espada(Clone)");
            if (_player != null)
            {
                _cinemachine.m_AnimatedTarget = _player.GetComponent<Animator>();
                _cinemachine.m_Follow = _player.transform;
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
