using System;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour {
    public static Action<float> OnSensivityChange;

    private Button _resume, _reset;
    private Slider _sensitivity;
    private float _sensitivityValue;

    private void OnEnable() {
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void GameManager_OnGamePaused(bool paused){
        if(paused){
            SetElements();
            _resume.clicked += ResumeClicked;
            _reset.clicked += ResetClicked;
            _sensitivity.value = GameManager.Instance.CurrentSensitivity;
            _sensitivity.RegisterCallback<ChangeEvent<float>>(SensitivityChanged);
        }else{
            _resume.clicked -= ResumeClicked;
            _reset.clicked -= ResetClicked;
        }
    }

    private void SetElements(){
        _resume = GameManager.Instance.UIManager.Root.Q<Button>("Resume");
        _reset = GameManager.Instance.UIManager.Root.Q<Button>("Reset");
        _sensitivity = GameManager.Instance.UIManager.Root.Q<Slider>("Slider");
    }

    private void ResumeClicked(){
        GameManager.OnGamePaused?.Invoke(false);
    }

    private void ResetClicked(){
        GameManager.OnGameEnd?.Invoke();
    }

    private void SensitivityChanged(ChangeEvent<float> evt){
        _sensitivityValue = evt.newValue;
        OnSensivityChange?.Invoke(_sensitivityValue);
    }
}