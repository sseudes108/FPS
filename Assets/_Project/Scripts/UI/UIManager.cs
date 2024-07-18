using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour {
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}
    
    public UIDocument UIDocument {get; private set;}
    public VisualElement Root {get; private set;}

    [SerializeField] private VisualTreeAsset _pausedAsset;
    [SerializeField] private VisualTreeAsset _victoryAsset;
    private VisualTreeAsset _defaultAsset;
    public UI_DeathScreen DeathScreen {get; private set;}
    public UI_Locus Locus {get; private set;}
    public UI_VictoryScreen VictoryScreen{get; private set;}
    
    private void OnEnable() {
        UIDocument = GetComponent<UIDocument>();
        SetRoot();
        _defaultAsset = UIDocument.visualTreeAsset;

        GameManager.OnGameOver.AddListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
        GameManager.OnGameFinished.AddListener(GameManager_OnGameFinished);
    }

    private void OnDisable() {
        GameManager.OnGameOver.RemoveListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
        GameManager.OnGameFinished.RemoveListener(GameManager_OnGameFinished);
    }

    private void Awake() {
        DeathScreen = GetComponent<UI_DeathScreen>();
        Locus = GetComponent<UI_Locus>();
        VictoryScreen = GetComponent<UI_VictoryScreen>();
    }
    
    private void GameManager_OnGameFinished(){
        UIDocument.visualTreeAsset = _victoryAsset;
        SetRoot();
        VictoryScreen.ShowVictoryScreenText(Root);
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

    private void SetRoot(){ //Root needs to be reset after each change of visual tree
        Root = UIDocument.rootVisualElement;
    }
}