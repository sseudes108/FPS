using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class UI_PauseMenu : UIManager {
    public static Action<float> OnSensivityChange;
    public static Action<Background> OnCrossChange;

    private Button _resume, _reset;
    private Slider _sensitivity;
    // private float _updatedSensitivity;
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
            _reset.clicked += ResetClicked;

            // _sensitivity.value = GameManager.Instance.CurrentSensitivity;
            _sensitivity.value = GameManager.Instance.DataManager.GameData.Sensitivity;
            _sensitivity.RegisterCallback<ChangeEvent<float>>(SensitivityChanged);

            foreach(Button button in buttons){
                button.clicked += () => OnButtonClick(button);
            }
        }else{

            _resume.clicked -= ResumeClicked;
            _reset.clicked -= ResetClicked;

            foreach(Button button in buttons){
                button.clicked -= () => OnButtonClick(button);
            }

            GameManager.Instance.DataManager.SaveGame();
        }
    }

    private void SetElements(){
        _resume = GameManager.Instance.UIManager.Root.Q<Button>("Resume");
        _reset = GameManager.Instance.UIManager.Root.Q<Button>("Reset");
        _sensitivity = GameManager.Instance.UIManager.Root.Q<Slider>("Slider");

        for (int i = 0; i < 6; i++){
            var button = GameManager.Instance.UIManager.Root.Q<Button>($"Cross{i+1}");
            buttons.Add(button);
        }
    }

    private void OnButtonClick(Button button){
        OnCrossChange?.Invoke(button.iconImage);
    }

    private void ResumeClicked(){
        GameManager.OnGamePaused?.Invoke(GameManager.Instance.DataManager.GameData, false);
    }

    private void ResetClicked(){
        GameManager.OnGameEnd?.Invoke();
    }

    private void SensitivityChanged(ChangeEvent<float> evt){
        // _updatedSensitivity = evt.newValue;
        _currentSensitivity = evt.newValue;
        OnSensivityChange?.Invoke(_currentSensitivity);
        // SaveData(GameManager.Instance.DataManager.GameData);
    }

    public void LoadData(GameData data){
        _currentSensitivity = data.Sensitivity;
    }

    public void SaveData(GameData data){}
}