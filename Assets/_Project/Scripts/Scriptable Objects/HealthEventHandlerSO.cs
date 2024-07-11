using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthEventHandlerSO", menuName = "FPS/EventHandlers/Health", order = 0)]
public class HealthEventHandlerSO : ScriptableObject {
    public  UnityEvent<int> OnHealthChange;
    public  UnityEvent OnPlayerDamaged;
    public  UnityEvent OnPlayerDied;
    
    private void OnEnable() {
        OnHealthChange ??= new UnityEvent<int>();
        OnPlayerDamaged ??= new UnityEvent();
        OnPlayerDied ??= new UnityEvent();
    }

    public void HealthChange(int health) { OnHealthChange?.Invoke(health); }
    public void PlayerDamaged() { OnPlayerDamaged?.Invoke(); }
    public void PlayerDied() { OnPlayerDied?.Invoke(); }
}