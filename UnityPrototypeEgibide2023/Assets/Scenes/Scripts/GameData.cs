using System.Collections;
using System.Collections.Generic;
using General.Scripts;
using UnityEngine;
[System.Serializable]
public struct GameData
{
    public Vector3 spawnPosition;
    public SceneObject spawnScene;
    public bool isValid;
    public List<int> collectedItems;
    public int CurrentHealth;
    public GameObject[] PotionList;
    public GameObject SelectedPotion;
    public bool AirDashUnlocked;
}
