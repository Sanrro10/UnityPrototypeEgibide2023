using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audios", menuName = "Audios", order = 5)]
public class Audios : ScriptableObject
{
    public List<AudioClip> audios = new List<AudioClip>();
}
