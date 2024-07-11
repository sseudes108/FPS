using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameEventHandlerSO", menuName = "FPS/EventHandlers/Game", order = 0)]
public class GameEventHandlerSO : ScriptableObject {
    public UnityEvent OnGameStart;
    public UnityEvent OnGameEnd;
    public UnityEvent<GameData, bool> OnGamePaused;

    private void OnEnable() {
        OnGameStart ??= new UnityEvent();
        OnGameEnd ??= new UnityEvent();
        OnGamePaused ??= new UnityEvent<GameData, bool>();
    }

    public void Start() { OnGameStart?.Invoke(); }
    public void End() { OnGameEnd?.Invoke(); }
    public void Paused(GameData gameData, bool isPaused) { OnGamePaused?.Invoke(gameData, isPaused); }
}