using System;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundSO", menuName = "FPS/Misc/Sound", order = 0)]
public class SoundSO : ScriptableObject {
    public enum AudioTypes{
        GameEffect = 0,
        UIEffect = 1,
        Music = 2
    }

    public AudioTypes AudioType;
    public AudioClip AudioClip;

    public bool Loop;
    public bool RandomizePitch;
    

    [Range(0f, 1f)]
    public float RandomPitchModifier = 0.1f;

    [Range(0.1f, 4f)]
    public float Volume = 0.1f;

    [Range(0f, 3f)]
    public float Pitch = 0.1f;
}