using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects", order = 1)]
public class PlayerData : ScriptableObject
{
    public float movementSpeed;
    public float jumpPower;
}

