using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    [field:SerializeField] public GameEventHandlerSO GameManager { get; private set;}
    
    public UIDocument UIDocument {get; private set;}
    public VisualElement Root {get; private set;}

    [SerializeField] private VisualTreeAsset _pausedAsset;
    private VisualTreeAsset _defaultAsset;
    public UI_DeathScreen DeathScreen {get; private set;}
    public UI_Locus Locus {get; private set;}
    
    private void OnEnable() {
        UIDocument = GetComponent<UIDocument>();
        SetRoot();
        _defaultAsset = UIDocument.visualTreeAsset;

        GameManager.OnGameEnd.AddListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GameManager.OnGameEnd.RemoveListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void Awake() {
        DeathScreen = GetComponent<UI_DeathScreen>();
        Locus = GetComponent<UI_Locus>();
    }

    private void GameManager_OnGamePaused(GameData data, bool isPaused){
        if(isPaused){
            UIDocument.visualTreeAsset = _pausedAsset;
        }else{
            UIDocument.visualTreeAsset = _defaultAsset;
        }
        SetRoot();
    }

    private void GameManager_OnGameEnd(){
        UIDocument.visualTreeAsset = _defaultAsset;
        SetRoot();
    }

    private void SetRoot(){ //Root needs to be reset after each change of visual tree
        Root = UIDocument.rootVisualElement;
    }
}