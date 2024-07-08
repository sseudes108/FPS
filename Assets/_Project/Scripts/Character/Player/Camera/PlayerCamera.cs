using UnityEngine;

public class PlayerCamera : MonoBehaviour, IDataPersistencer {
    [SerializeField] private Transform _cameraTarget;
    [SerializeField] private FirstPersonCamera _firstPersonCamera;

    [Range(0.01f, 18)]
    [SerializeField] private float _sensitivity;
    public float Sensitivity => _sensitivity;
    private float _rotationValue = 0f;

    private void OnEnable() {
        UI_PauseMenu.OnSensivityChange += OnPauseMenu_OnSensivityChange;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        UI_PauseMenu.OnSensivityChange -= OnPauseMenu_OnSensivityChange;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    public void CameraRotation(float mouseInput){
        _rotationValue -= mouseInput;
        _rotationValue = Mathf.Clamp(_rotationValue, -60f, 60f);
        _cameraTarget.localRotation = Quaternion.Euler(_rotationValue, 0f, 0f);
    }

    public Transform GetCameraTransform(){
        return _firstPersonCamera.transform;
    }

    private void OnPauseMenu_OnSensivityChange(float value){
        _sensitivity = value;
    }

    private void GameManager_OnGamePaused(GameData data, bool paused){
        if(!paused){
            GameManager.Instance.DataManager.SaveGame();
        }
    }

    public void LoadData(GameData gameData){
        _sensitivity = gameData.Sensitivity;
    }

    public void SaveData(ref GameData gameData){
        gameData.Sensitivity = _sensitivity;
    }
}