using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "PauseMenuEventHandlerSO", menuName = "FPS/EventHandlers/PauseMenu", order = 0)]
public class PauseMenuEventHandlerSO : ScriptableObject {
    public UnityEvent<float> OnSensivityChange;
    public UnityEvent<Background, int> OnCrossChange;

    private void OnEnable() {
        OnSensivityChange ??= new UnityEvent<float>();
        OnCrossChange ??= new UnityEvent<Background, int>();
    }

    public void SensitivityChanged(float value) { OnSensivityChange?.Invoke(value); }
    public void CrossChanged(Background newCross, int crossIndex) { OnCrossChange?.Invoke(newCross, crossIndex); }
}