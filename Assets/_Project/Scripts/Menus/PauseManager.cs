using UnityEngine;

public class PauseManager : MonoBehaviour {
    private bool _isPaused;
    public bool IsPaused => _isPaused;

    private void OnEnable() {
        GameManager.OnGamePaused += GameManager_OnGamePaused;
        GameManager.OnGameEnd += GameManager_OnGameEnd;
    }

    private void OnDisable() {
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
        GameManager.OnGameEnd -= GameManager_OnGameEnd;
    }

    private void Start() {
        _isPaused = false;
    }

    private void GameManager_OnGamePaused(GameData data, bool paused){
        _isPaused = paused;
        
        if(_isPaused){
            Time.timeScale = 0f;
        }else{
            Time.timeScale = 1f;
        }
    }

    private void GameManager_OnGameEnd(){        
        Time.timeScale = 1f;
    }
}