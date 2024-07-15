using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "HealthManagerSO", menuName = "FPS/Managers/Health", order = 0)]
public class HealthManagerSO : ScriptableObject {
    [HideInInspector] public  UnityEvent<int> OnHealthChange;
    [HideInInspector] public  UnityEvent OnPlayerDamaged;
    [HideInInspector] public  UnityEvent OnPlayerDied;
    
    private void OnEnable() {
        OnHealthChange ??= new UnityEvent<int>();
        OnPlayerDamaged ??= new UnityEvent();
        OnPlayerDied ??= new UnityEvent();
    }

    public void HealthChange(int health) { OnHealthChange?.Invoke(health); }
    public void PlayerDamaged() { OnPlayerDamaged?.Invoke(); }
    public void PlayerDied() { OnPlayerDied?.Invoke(); }
}