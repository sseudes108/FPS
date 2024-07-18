using UnityEngine;

[System.Serializable]
public class GameData {
    public int CrossHair;

    [Range(0.5f, 18f)]
    public float Sensitivity;
    public Vector3 RespawnPosition;

    public float MusicVolume;
    public float EffectVolume;

    public GameData(){
        RespawnPosition = new Vector3(-20.15f, 1.2f, -23.4f);
        CrossHair = 0;
        Sensitivity = 10;
        MusicVolume = 72f;
        EffectVolume = 72f;
    }
}