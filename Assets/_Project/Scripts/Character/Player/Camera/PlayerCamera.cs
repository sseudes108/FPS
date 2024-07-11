using UnityEngine;

public class PlayerCamera : MonoBehaviour, IDataPersistencer {
    [field:SerializeField] public PauseMenuEventHandlerSO PauseMenuManager { get; private set; }
    [field:SerializeField] public GameEventHandlerSO GameManager { get; private set;}
    private Transform _cameraTarget;
    private FirstPersonCamera _firstPersonCamera;

    [Range(0.01f, 18)]
    [SerializeField] private float _sensitivity;
    public float Sensitivity => _sensitivity;
    private float _rotationValue = 0f;

    private void OnEnable() {
        // UI_PauseMenu.OnSensivityChange += OnPauseMenu_OnSensivityChange;
        PauseMenuManager.OnSensivityChange.AddListener(PauseMenuManager_OnSensivityChange);
        // GameController.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        // UI_PauseMenu.OnSensivityChange -= OnPauseMenu_OnSensivityChange;
        PauseMenuManager.OnSensivityChange.RemoveListener(PauseMenuManager_OnSensivityChange);
        // GameController.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void Awake() {
        _cameraTarget = transform.Find("Model/Camera/CamTarget");
        _firstPersonCamera = transform.Find("Model/Camera/FPS Cam").GetComponent<FirstPersonCamera>();
    }

    public void CameraRotation(float mouseInput){
        _rotationValue -= mouseInput;
        _rotationValue = Mathf.Clamp(_rotationValue, -60f, 60f);
        _cameraTarget.localRotation = Quaternion.Euler(_rotationValue, 0f, 0f);
    }

    public Transform GetCameraTransform(){
        return _firstPersonCamera.transform;
    }

    private void PauseMenuManager_OnSensivityChange(float value){
        _sensitivity = value;
    }

    private void GameManager_OnGamePaused(GameData data, bool paused){
        if(!paused){
            GameController.Instance.DataManager.SaveGame();
        }
    }

    public void LoadData(GameData gameData){
        _sensitivity = gameData.Sensitivity;
    }

    public void SaveData(ref GameData gameData){
        gameData.Sensitivity = _sensitivity;
    }
}