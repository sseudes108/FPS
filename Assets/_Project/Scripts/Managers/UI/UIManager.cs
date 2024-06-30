using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    public UIDocument UIDocument {get; private set;}
    public VisualElement Root {get; private set;}

    [SerializeField] private VisualTreeAsset _pausedAsset;
    [SerializeField] private VisualTreeAsset _defaultAsset;
    
    private void OnEnable() {
        UIDocument = GetComponent<UIDocument>();
        SetRoot();
        _defaultAsset = UIDocument.visualTreeAsset;

        GameManager.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
    }

    private void OnDisable() {
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
    }

    private void GameManager_OnGamePaused(bool isPaused){
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

    private void SetRoot(){
        Root = UIDocument.rootVisualElement;
    }
}