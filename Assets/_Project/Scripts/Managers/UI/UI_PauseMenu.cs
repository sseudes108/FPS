using UnityEngine;
using UnityEngine.UIElements;

public class UI_PauseMenu : MonoBehaviour {
    private Button _resume, _reset;

    private void OnEnable() {
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
    }

    private void GameManager_OnGamePaused(bool paused){
        if(paused){
            SetButtons();
            _resume.clicked += ResumeClicked;
            _reset.clicked += ResetClicked;
        }else{
            _resume.clicked -= ResumeClicked;
            _reset.clicked -= ResetClicked;
        }
    }

    private void SetButtons(){
        _resume = GameManager.Instance.UIManager.Root.Q<Button>("Resume");
        _reset = GameManager.Instance.UIManager.Root.Q<Button>("Reset");
    }

    private void ResumeClicked(){
        GameManager.OnGamePaused?.Invoke(false);
    }

    private void ResetClicked(){
        GameManager.OnGameEnd?.Invoke();
    }
}