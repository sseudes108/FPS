using UnityEngine;
using UnityEngine.UIElements;

public class UI_Locus : MonoBehaviour {
    public VisualElement _crossHair;
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private Color _crosshairColor;

    private Label _currentAmmo;
    private Label _maxAmmo;
    private Label _fps;    
    private Label _sensitivity;    

    private void OnEnable() {
        PlayerGun.OnAmmoCountChange += PlayerGun_OnAmmoCountChange;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        PlayerGun.OnAmmoCountChange -= PlayerGun_OnAmmoCountChange;
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void Start() {
        SetElements();
        CrossHairStyleConfig();
    }

    void Update() {
        float frameRate = 1.0f / Time.deltaTime;

        if (_fps != null){
            _fps.text = $"FPS: {Mathf.RoundToInt(frameRate)}";
        }   
    }

    private void GameManager_OnGamePaused(bool paused){ //Reset The elements after the change in style asset from pause menu
        if(!paused){
            SetElements();
            if (_sensitivity != null){
                _sensitivity.text = $"Sensitivity: {GameManager.Instance.CurrentSensitivity}";
            }
        }
    }

    private void PlayerGun_OnAmmoCountChange(int ammoLeftInMag, int magazineSize, int maxAmmo){
        _currentAmmo.text = $"{ammoLeftInMag}/{magazineSize}";
        _maxAmmo.text = $"{maxAmmo}";
    }

    private void CrossHairStyleConfig(){
        _crossHair.style.backgroundImage = _crosshairTexture;
        _crossHair.style.unityBackgroundImageTintColor = _crosshairColor;
    }

    private void SetElements(){
        _crossHair = GameManager.Instance.UIManager.Root.Q("Cross");
        _currentAmmo = GameManager.Instance.UIManager.Root.Q<Label>("CurrentAmmoLabel");
        _maxAmmo = GameManager.Instance.UIManager.Root.Q<Label>("MaxAmmoLabel");

        _fps = GameManager.Instance.UIManager.Root.Q<Label>("FPS");
        _sensitivity = GameManager.Instance.UIManager.Root.Q<Label>("Sensitivity");
    }
}