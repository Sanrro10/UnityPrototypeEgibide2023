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
    private GameController.SPlayerSpawnData _spawnData;

    private void Awake()
    {
        _spawnData = new GameController.SPlayerSpawnData();
        _spawnData.Scene = scene;
        _spawnData.Position = spawnPoint;
        _spawnData.GoToPosition = goToPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if ( other.GetComponent<PlayerController>().PmStateMachine.CurrentState is SceneChangeState)
            {
                return;
            }
            GameController.Instance.SceneLoad(_spawnData,false);
        }
    }
}
