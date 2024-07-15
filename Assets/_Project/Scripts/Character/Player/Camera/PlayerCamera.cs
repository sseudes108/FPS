using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    [field:SerializeField] public PauseMenuManagerSO PauseMenuManager { get; private set; }
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}
    private Transform _cameraTarget;
    private FirstPersonCamera _firstPersonCamera;

    [field:SerializeField] public  DataManagerSO DataManager { get; private set;}

    [Range(0.01f, 18)]
    [SerializeField] private float _sensitivity;
    public float Sensitivity => _sensitivity;
    private float _rotationValue = 0f;

    private void OnEnable() {
        PauseMenuManager.OnSensivityChange.AddListener(PauseMenuManager_OnSensivityChange);
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        PauseMenuManager.OnSensivityChange.RemoveListener(PauseMenuManager_OnSensivityChange);
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

    private void GameManager_OnGamePaused(bool paused){
        if(!paused){
            DataManager.SaveSensitivity(_sensitivity);
        }
    }
}