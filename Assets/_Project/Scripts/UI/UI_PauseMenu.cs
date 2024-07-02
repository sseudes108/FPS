using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour {
    public static Action<float> OnSensivityChange;
    public static Action<Background> OnCrossChange;

    private Button _resume, _reset;
    private Slider _sensitivity;
    private float _sensitivityValue;

    public List<Button> buttons = new ();

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

            foreach(Button button in buttons){
                button.clicked += () => OnButtonClick(button);
            }
        }else{

            _resume.clicked -= ResumeClicked;
            _reset.clicked -= ResetClicked;

            foreach(Button button in buttons){
                button.clicked -= () => OnButtonClick(button);
            }
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
        // Debug.Log($"{button.name} clicked");
        OnCrossChange?.Invoke(button.iconImage);

        // switch (button.name){
        //     case "Cross1":
        //         var Texture2D = button.iconImage;
        //     break;

        //     case "Cross2":
        //     break;

        //     case "Cross3":
        //     break;

        //     case "Cross4":
        //     break;

        //     case "Cross5":
        //     break;

        //     case "Cross6":
        //     break;

        //     default:
        //     break;

        // }
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