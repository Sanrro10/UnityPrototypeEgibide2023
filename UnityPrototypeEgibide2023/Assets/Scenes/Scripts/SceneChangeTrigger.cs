using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player.Scripts;
using Entities.Player.Scripts.StatePattern.PlayerStates;
using General.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private SceneObject scene;
    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] private Vector3 goToPosition;
    public bool onTheLeft;
    public bool useSceneChangeTrigger = true;
    
    
    private GameController.SPlayerSpawnData _spawnData;

    private void Awake()
    {
        _spawnData = new GameController.SPlayerSpawnData();
        _spawnData.Scene = scene;
        _spawnData.Position = spawnPoint;
        _spawnData.GoToPosition = goToPosition;
        _spawnData.OnTheLeft = onTheLeft;
        _spawnData.UseSceneChangeTrigger = useSceneChangeTrigger;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var pc = other.GetComponent<PlayerController>();
            if (pc == null) return;
            if (pc.PmStateMachine.CurrentState is SceneChangeState) return;
            GameController.Instance.SceneLoad(_spawnData,false);
        }
    }
}
