using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_Locus : MonoBehaviour, IDataPersistencer {
    [field:SerializeField] public GunEventHandlerSO GunManager  { get; private set; }
    [field:SerializeField] public VisualsEventHandlerSO VisualsManager  { get; private set; }
    [field:SerializeField] public PauseMenuEventHandlerSO PauseMenuManager { get; private set; }
    [field:SerializeField] public GameEventHandlerSO GameManager { get; private set;}

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
        GunManager.OnAmmoCountChange.AddListener(GunManager_OnAmmoCountChange);
        // GameController.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
        PauseMenuManager.OnCrossChange.AddListener(PauseMenuManager_OnCrossChange);
    }

    private void OnDisable() {
        GunManager.OnAmmoCountChange.RemoveListener(GunManager_OnAmmoCountChange);
        // GameController.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
        PauseMenuManager.OnCrossChange.RemoveListener(PauseMenuManager_OnCrossChange);
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

    private void GunManager_OnAmmoCountChange(int ammoLeftInMag, int magazineSize, int maxAmmo){
        _currentAmmo.text = $"Ammo : {ammoLeftInMag:00}/{magazineSize}";
        _maxAmmo.text = $"{maxAmmo}";
    }

    private void CrossHairStyleConfig(){
        _crossHair.style.backgroundImage = _updatedCrossHair;
        _crossHair.style.unityBackgroundImageTintColor = _crosshairColor;
    }

    private void SetElements(){
        _canvas = GameController.Instance.UIManager.Root.Q("Canvas");
        _crossHair = GameController.Instance.UIManager.Root.Q("Cross");
        _currentAmmo = GameController.Instance.UIManager.Root.Q<Label>("CurrentAmmoLabel");
        _maxAmmo = GameController.Instance.UIManager.Root.Q<Label>("MaxAmmoLabel");
    }

    //Background newCross
    private void PauseMenuManager_OnCrossChange(Background newCross, int crossIndex){
        _updatedCrossHair = newCross;
        _crosshairIndex = crossIndex;
        crossChanged = true;
        GameController.Instance.DataManager.SaveGame();
    }

    public void LoadData(GameData data){
        _updatedCrossHair = VisualsManager.CrossesTextures[data.CrossHair];
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