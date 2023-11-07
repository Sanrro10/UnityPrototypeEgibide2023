using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Vector3 spawnPoint;
    private GameController.SPlayerSpawnData _spawnData;

    private void Awake()
    {
        _spawnData = new GameController.SPlayerSpawnData();
        _spawnData.SceneName = sceneName;
        _spawnData.Position = spawnPoint;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameController.Instance.SceneLoad(_spawnData);
        }
    }
}
