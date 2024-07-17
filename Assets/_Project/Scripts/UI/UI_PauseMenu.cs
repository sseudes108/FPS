using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour {
    [field:SerializeField] public AudioManagerSO AudioManager { get; private set; }
    [field:SerializeField] public VisualManagerSO VisualsManager { get; private set; }
    [field:SerializeField] public PauseMenuManagerSO PauseMenuManager { get; private set; }
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}
    [field:SerializeField] public  DataManagerSO DataManager { get; private set;}

    private Button _resume, _reset;
    private Slider _sensitivity;
    private Slider _musicSlider;
    private Slider _effectSlider;
    private float _currentSensitivity;
    private List<Button> buttons = new();

    private void OnEnable() {
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void GameManager_OnGamePaused(bool paused){
        if(paused){
            SetElements();
            _resume.clicked += ResumeClicked;
            _reset.clicked += MainMenuClicked;

            _sensitivity.value = DataManager.LoadSensitivity();
            _sensitivity.RegisterCallback<ChangeEvent<float>>(SensitivityChanged);
            
            _musicSlider.value = DataManager.LoadMusicVolume();
            _musicSlider.RegisterCallback<ChangeEvent<float>>(MusicVolumeChanged);

            _effectSlider.value = DataManager.LoadEffectVolume();
            _effectSlider.RegisterCallback<ChangeEvent<float>>(EffectVolumeChanged);

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
        _musicSlider = GameController.Instance.UIManager.Root.Q<Slider>("MusicSlider");
        _effectSlider = GameController.Instance.UIManager.Root.Q<Slider>("EffectSlider");

        for (int i = 0; i < 6; i++){
            var button = GameController.Instance.UIManager.Root.Q<Button>($"Cross{i+1}");
            buttons.Add(button);
        }
    }

    private void OnCrossButtonClick(Button button){
        var index = buttons.IndexOf(button);
        PauseMenuManager.CrossChanged(button.iconImage, index);
        AudioManager.PlayClickSound(this);
    }

    private void ResumeClicked(){
        AudioManager.PlayClickSound(this);
        GameManager.Pause(false);
    }

    private void MainMenuClicked(){
        AudioManager.PlayClickSound(this);
        VisualsManager.FadeToBlack(1f);
        GameManager.GameOver();
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

    private void MusicVolumeChanged(ChangeEvent<float> evt){
        PauseMenuManager.MusicVolumeChanged(evt.newValue);
    }

    private void EffectVolumeChanged(ChangeEvent<float> evt){
        PauseMenuManager.EffectVolumeChanged(evt.newValue);
    }
}