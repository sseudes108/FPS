using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_PauseMenu : UIManager {
    public static Action<float> OnSensivityChange;
    public static Action<Background, int> OnCrossChange;

    private Button _resume, _reset;
    private Slider _sensitivity;
    private float _currentSensitivity;

    public List<Button> buttons = new ();

    private void OnEnable() {
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void GameManager_OnGamePaused(GameData data, bool paused){
        if(paused){
            SetElements();
            _resume.clicked += ResumeClicked;
            _reset.clicked += MainMenuClicked;

            _sensitivity.value = GameManager.Instance.DataManager.GameData.Sensitivity;
            _sensitivity.RegisterCallback<ChangeEvent<float>>(SensitivityChanged);

            foreach(Button button in buttons){
                button.clicked += () => OnButtonClick(button);
            }
        }else{

            _resume.clicked -= ResumeClicked;
            _reset.clicked -= MainMenuClicked;

            foreach(Button button in buttons){
                button.clicked -= () => OnButtonClick(button);
            }

        }
    }

    private void SetElements(){
        buttons.Clear();
        _resume = GameManager.Instance.UIManager.Root.Q<Button>("Resume");
        _reset = GameManager.Instance.UIManager.Root.Q<Button>("Reset");
        _sensitivity = GameManager.Instance.UIManager.Root.Q<Slider>("Slider");

        for (int i = 0; i < 6; i++){
            var button = GameManager.Instance.UIManager.Root.Q<Button>($"Cross{i+1}");
            buttons.Add(button);
        }
    }

    private void OnButtonClick(Button button){
        var index = buttons.IndexOf(button);
        OnCrossChange?.Invoke(button.iconImage, index);
    }

    private void ResumeClicked(){
        GameManager.OnGamePaused?.Invoke(GameManager.Instance.DataManager.GameData, false);
    }

    private void MainMenuClicked(){
        GameManager.Instance.Visual.Effects.FadeScreen.FadeScreenToBlack(1f);
        GameManager.OnGameEnd?.Invoke();
        StartCoroutine(MainMenuRoutine());
    }

    private IEnumerator MainMenuRoutine(){
        yield return new WaitForSeconds(1f);
        GameManager.Instance.LoadMainMenu();
    }

    private void SensitivityChanged(ChangeEvent<float> evt){
        _currentSensitivity = evt.newValue;
        OnSensivityChange?.Invoke(_currentSensitivity);
    }
}