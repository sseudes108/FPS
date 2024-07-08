using UnityEngine;

[System.Serializable]
public class GameData {
    public int CrossHair;

    [Range(0.5f, 18)]
    public float Sensitivity;
    
    public Vector3 RespawnPosition;

    public GameData(){
        CrossHair = 0;
        Sensitivity = 10;
    }
}