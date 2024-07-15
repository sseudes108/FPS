using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameManagerSO", menuName = "FPS/Managers/Game", order = 3)]
public class GameManagerSO : ScriptableObject {
    [HideInInspector] public UnityEvent OnGameStart;
    [HideInInspector] public UnityEvent OnGameOver;
    [HideInInspector] public UnityEvent<bool> OnGamePaused;
    [HideInInspector] public UnityEvent OnGameFinished;
    public Vector2 RotationInput { get; private set; }
    public bool _gameOver = false;

    private void OnEnable() {
        OnGameStart ??= new UnityEvent();
        OnGameOver ??= new UnityEvent();
        OnGamePaused ??= new UnityEvent<bool>();
        OnGameFinished ??= new UnityEvent();     
    }

    public void GameStart() {
        _gameOver = false;
        OnGameStart?.Invoke(); 
    }

    public void GameOver() {
        if(_gameOver) { return; }
        _gameOver = true;
        OnGameOver?.Invoke(); 
    }

    public void GameFinished() {
        if(_gameOver) { return; }
        _gameOver = true;
        OnGameFinished?.Invoke(); 
    }

    public void Pause(bool isPaused) {
        if(_gameOver) { return; }
        OnGamePaused?.Invoke(isPaused);
    }
    public void UpdateRotationInput(float mouseX, float mouseY) { RotationInput = new Vector2(mouseX, mouseY); }
}