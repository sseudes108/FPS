using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Locus : MonoBehaviour, IDataPersistencer {
    private VisualElement _crossHair;
    private VisualElement _canvas;

    [SerializeField] private Color _crosshairColor;

    //User to change the crosshair in pause menu
    private Background _updatedCrossHair;
    private int _crosshairIndex;

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
        _canvas.style.opacity = 0;
        CrossHairStyleConfig();
    }

    public void MakeScreenVisible(){
        StartCoroutine(ElementOpacityRoutine(_canvas, 0, 1, 1f));
    }

    private void GameManager_OnGamePaused(GameData data, bool paused){ //Reset The elements after the change in style asset from pause menu
        if(!paused){
            SetElements();
            if(crossChanged){
                _crossHair.style.backgroundImage = _updatedCrossHair;
                crossChanged = false;
                crossHasChanged = true;
            }

            if(crossHasChanged){
                _crossHair.style.backgroundImage = _updatedCrossHair;
            }
        }
    }

    private void PlayerGun_OnAmmoCountChange(int ammoLeftInMag, int magazineSize, int maxAmmo){
        _currentAmmo.text = $"Ammo : {ammoLeftInMag:00}/{magazineSize}";
        _maxAmmo.text = $"{maxAmmo}";
    }

    private void CrossHairStyleConfig(){
        _crossHair.style.backgroundImage = _updatedCrossHair;
        _crossHair.style.unityBackgroundImageTintColor = _crosshairColor;
    }

    private void SetElements(){
        _canvas = GameManager.Instance.UIManager.Root.Q("Canvas");
        _crossHair = GameManager.Instance.UIManager.Root.Q("Cross");
        _currentAmmo = GameManager.Instance.UIManager.Root.Q<Label>("CurrentAmmoLabel");
        _maxAmmo = GameManager.Instance.UIManager.Root.Q<Label>("MaxAmmoLabel");
    }

    //Background newCross
    private void UI_PauseMenu_OnCrossChange(Background newCross, int crossIndex){
        _updatedCrossHair = newCross;
        _crosshairIndex = crossIndex;
        crossChanged = true;
        GameManager.Instance.DataManager.SaveGame();
    }

    public void LoadData(GameData data){
        _updatedCrossHair = GameManager.Instance.Visual.CrossesTextures[data.CrossHair];
        _crosshairIndex = data.CrossHair;
    }

    public void SaveData(ref GameData data){
        data.CrossHair = _crosshairIndex;
    }

    private IEnumerator ElementOpacityRoutine(VisualElement element, float start, float end, float duration){
        float time = 0f;
        float currentOpacity;
        do{
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            currentOpacity  = Mathf.Lerp(start, end, t);
            element.style.opacity = currentOpacity;
            yield return null;
        }while(currentOpacity > 0);
        yield return null;
    }
}