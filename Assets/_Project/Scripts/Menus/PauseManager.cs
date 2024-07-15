using UnityEngine;

public class PauseManager : MonoBehaviour {
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}

    private bool _isPaused;
    public bool IsPaused => _isPaused;

    private void OnEnable() {
        GameManager.OnGameOver.AddListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GameManager.OnGameOver.RemoveListener(GameManager_OnGameEnd);
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void Start() {  _isPaused = false; }

    private void GameManager_OnGamePaused(bool paused){
        _isPaused = paused;
        
        if(_isPaused){
            Time.timeScale = 0f;
        }else{
            Time.timeScale = 1f;
        }
    }
    private void GameManager_OnGameEnd() { Time.timeScale = 1f; }
}