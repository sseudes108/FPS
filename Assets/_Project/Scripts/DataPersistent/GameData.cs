using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class GameData {
    public Background CrossHair;
    public float Sensitivity;
    public Vector3 RespawnPosition;

    public GameData(){
        Sensitivity = 9f;   
        CrossHair = null;
        RespawnPosition = Vector3.zero;
    }
}