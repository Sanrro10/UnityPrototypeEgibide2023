using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects", order = 1)]
public class PlayerData : ScriptableObject
{
    public float movementSpeed;
    public float jumpPower;
    public bool facingRight;
    public float downAttack;
    public float dashSpeed;
    public float dashDuration;
    public float dashCooldown;
    public float gravity;
    public float potionColdownTime;
    public float jumpDuration;
    public float floatDuration;
    public float airdashDuration;
    public float airdashForce;
}

