using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour {
    [field:SerializeField] public AudioEventHandlerSO AudioManager { get; private set; }
    [field:SerializeField] public VisualsEventHandlerSO VisualsManager { get; private set; }
    [field:SerializeField] public PauseMenuEventHandlerSO PauseMenuManager { get; private set; }
    [field:SerializeField] public GameEventHandlerSO GameManager { get; private set;}

    private Button _resume, _reset;
    private Slider _sensitivity;
    private float _currentSensitivity;
    private List<Button> buttons = new ();

    private void OnEnable() {
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void GameManager_OnGamePaused(GameData data, bool paused){
        if(paused){
            SetElements();
            _resume.clicked += ResumeClicked;
            _reset.clicked += MainMenuClicked;

            _sensitivity.value = GameController.Instance.DataManager.GameData.Sensitivity;
            _sensitivity.RegisterCallback<ChangeEvent<float>>(SensitivityChanged);

            foreach(Button button in buttons){
                button.clicked += () => OnCrossButtonClick(button);
            }
        }else{

            _resume.clicked -= ResumeClicked;
            _reset.clicked -= MainMenuClicked;

            foreach(Button button in buttons){
                button.clicked -= () => OnCrossButtonClick(button);
            }

        }
    }

    private void SetElements(){
        buttons.Clear();
        _resume = GameController.Instance.UIManager.Root.Q<Button>("Resume");
        _reset = GameController.Instance.UIManager.Root.Q<Button>("Reset");
        _sensitivity = GameController.Instance.UIManager.Root.Q<Slider>("Slider");

        for (int i = 0; i < 6; i++){
            var button = GameController.Instance.UIManager.Root.Q<Button>($"Cross{i+1}");
            buttons.Add(button);
        }
    }

    private void OnCrossButtonClick(Button button){
        var index = buttons.IndexOf(button);
        PauseMenuManager.CrossChanged(button.iconImage, index);
        AudioManager.PlayClickSound();
    }

    private void ResumeClicked(){
        AudioManager.PlayClickSound();
        GameManager.Paused(GameController.Instance.DataManager.GameData, false);
    }

    private void MainMenuClicked(){
        AudioManager.PlayClickSound();
        VisualsManager.FadeToBlack(1f);
        GameManager.End();
        StartCoroutine(MainMenuRoutine());
    }

    private IEnumerator MainMenuRoutine(){
        yield return new WaitForSeconds(1f);
        GameController.Instance.LoadMainMenu();
    }

    private void SensitivityChanged(ChangeEvent<float> evt){
        _currentSensitivity = evt.newValue;
        PauseMenuManager.SensitivityChanged(_currentSensitivity);
    }
}