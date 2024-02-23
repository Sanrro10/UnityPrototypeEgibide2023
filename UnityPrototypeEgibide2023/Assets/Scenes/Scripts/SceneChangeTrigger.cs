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
    public bool requiredInputPress = false;
    
    
    private GameController.SPlayerSpawnData _spawnData;
    public static event Action OnSceneChangeOverlap;
    public static event Action OnSceneChangeExit;
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
        if (requiredInputPress || !other.CompareTag("Player")) return;
        ChangeScene(other.GetComponent<PlayerController>());
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!requiredInputPress || !other.CompareTag("Player")) return;
        OnSceneChangeOverlap?.Invoke();
        if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.JoystickButton12) || Input.GetKey(KeyCode.JoystickButton19) ||  Input.GetKey(KeyCode.JoystickButton3)) ChangeScene(other.GetComponent<PlayerController>());
    }

    private void OnTriggerExit2D(Collider2D other) => OnSceneChangeExit?.Invoke();

    private void ChangeScene(PlayerController pc)
    {
        if (pc == null) return;
        if (pc.PmStateMachine.CurrentState is SceneChangeState) return;
        GameController.Instance.SceneLoad(_spawnData,false);
    }
}
