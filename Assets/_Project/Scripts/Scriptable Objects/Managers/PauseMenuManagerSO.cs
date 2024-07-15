using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PauseMenuManagerSO", menuName = "FPS/Managers/Pause Menu", order = 0)]
public class PauseMenuManagerSO : ScriptableObject {
    [HideInInspector] public UnityEvent<float> OnSensivityChange;
    [HideInInspector] public UnityEvent<Background, int> OnCrossChange;

    private void OnEnable() {
        OnSensivityChange ??= new UnityEvent<float>();
        OnCrossChange ??= new UnityEvent<Background, int>();
    }

    public void SensitivityChanged(float value) { OnSensivityChange?.Invoke(value); }
    public void CrossChanged(Background newCross, int crossIndex) { OnCrossChange?.Invoke(newCross, crossIndex); }
}