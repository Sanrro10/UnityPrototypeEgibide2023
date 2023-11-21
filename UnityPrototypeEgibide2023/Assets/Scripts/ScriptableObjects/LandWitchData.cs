using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LandWitchData" , menuName = "LandWitchData", order = 5)]
public class LandWitchData : ScriptableObject
{
    public int health;
    public float missileSpeed;
    public float missileCooldown;
    public float magicCircleCooldown;
    public float magicCircleChargeDuration;
    public float magicCircleEffectDuration;
}
