using UnityEngine;
using UnityEngine.UIElements;

public class UI_Locus : MonoBehaviour, IDataPersistencer {
    private VisualElement _crossHair;
    [SerializeField] private Texture2D _crosshairTexture;
    [SerializeField] private Color _crosshairColor;

    //User to change the crosshair in pause menu
    private Background updatedCrossHair;
    private bool crossChanged; //Seted wen the new cross is selected
    private bool crossHasChanged = false; //If no cross was selected in pause menu the original texture would be reaplied. This variable locks it.

    private Label _currentAmmo;
    private Label _maxAmmo;

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

    private void GameManager_OnGamePaused(GameData data, bool paused){ //Reset The elements after the change in style asset from pause menu
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

            GameManager.Instance.DataManager.SaveGame();
        }
    }

    private void PlayerGun_OnAmmoCountChange(int ammoLeftInMag, int magazineSize, int maxAmmo){
        _currentAmmo.text = $"{ammoLeftInMag}/{magazineSize}";
        _maxAmmo.text = $"{maxAmmo}";
    }

    private void CrossHairStyleConfig(){
        if(updatedCrossHair == null){
            _crossHair.style.backgroundImage = _crosshairTexture;
        }else{
            _crossHair.style.backgroundImage = updatedCrossHair;
        }
        _crossHair.style.unityBackgroundImageTintColor = _crosshairColor;
    }

    private void SetElements(){
        _crossHair = GameManager.Instance.UIManager.Root.Q("Cross");
        _currentAmmo = GameManager.Instance.UIManager.Root.Q<Label>("CurrentAmmoLabel");
        _maxAmmo = GameManager.Instance.UIManager.Root.Q<Label>("MaxAmmoLabel");
    }

    private void UI_PauseMenu_OnCrossChange(Background newCross){
        updatedCrossHair = newCross;
        crossChanged = true;
        SaveData(GameManager.Instance.DataManager.GameData);
    }

    public void LoadData(GameData data){
        updatedCrossHair = data.CrossHair;
    }

    public void SaveData(GameData data){
        data.CrossHair = updatedCrossHair;
    }
}