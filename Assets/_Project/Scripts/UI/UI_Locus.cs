using UnityEngine;
using UnityEngine.UIElements;

public class UI_Locus : MonoBehaviour {
    private VisualElement _crossHair;
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private Color _crosshairColor;

    //User to change the crosshair in pause menu
    private Background updatedCrossHair;
    private bool crossChanged; //Seted wen the new cross is selected
    private bool crossHasChanged = false; //If no cross was selected in pause menu the original texture would be reaplied. This variable locks it.

    private Label _currentAmmo;
    private Label _maxAmmo;
    // private Label _fps;    
    // private Label _sensitivity;

    private void OnEnable() {
        PlayerGun.OnAmmoCountChange += PlayerGun_OnAmmoCountChange;
        GameManager.OnGamePaused += GameManager_OnGamePaused;
        UI_PauseMenu.OnCrossChange += UI_PauseMenu_OnCrossChange;
    }

    private void OnDisable() {
        PlayerGun.OnAmmoCountChange -= PlayerGun_OnAmmoCountChange;
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
        UI_PauseMenu.OnCrossChange -= UI_PauseMenu_OnCrossChange;
    }

    private void Start() {
        SetElements();
        CrossHairStyleConfig();
    }

    // void Update() {
    //     float frameRate = 1.0f / Time.deltaTime;

    //     if (_fps != null){
    //         _fps.text = $"FPS: {Mathf.RoundToInt(frameRate)}";
    //     }   
    // }

    private void GameManager_OnGamePaused(bool paused){ //Reset The elements after the change in style asset from pause menu
        if(!paused){
            SetElements();
            if(crossChanged){
                _crossHair.style.backgroundImage = updatedCrossHair;
                crossChanged = false;
                crossHasChanged = true;
            }

            if(crossHasChanged){
                _crossHair.style.backgroundImage = updatedCrossHair;
            }
            
            // if (_sensitivity != null){
            //     _sensitivity.text = $"Sensitivity: {GameManager.Instance.CurrentSensitivity}";
            // }
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

        // _fps = GameManager.Instance.UIManager.Root.Q<Label>("FPS");
        // _sensitivity = GameManager.Instance.UIManager.Root.Q<Label>("Sensitivity");
    }

    private void UI_PauseMenu_OnCrossChange(Background newCross){
        updatedCrossHair = newCross;
        crossChanged = true;
    }

}