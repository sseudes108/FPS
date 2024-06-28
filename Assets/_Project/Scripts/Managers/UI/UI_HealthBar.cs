using UnityEngine;
using UnityEngine.UIElements;

public class UI_HealthBar : MonoBehaviour {
    private VisualElement _HBforeground;
    private int _lentgh = 100;

    private void OnEnable() {
        Health.OnHealthChange += Health_OnHealthChange;
    }

    private void OnDisable() {
        Health.OnHealthChange -= Health_OnHealthChange;
    }

    private void Start() {
        _HBforeground = GameManager.Instance.UIManager.Root.Q("HBFore");
        UpdateHealthBar();
    }

    private void Health_OnHealthChange(int currentHP){
        _lentgh = currentHP;
        if (_lentgh <= 0){
            _lentgh = 0;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar(){
        _HBforeground.style.width = Length.Percent(_lentgh);
    }
}