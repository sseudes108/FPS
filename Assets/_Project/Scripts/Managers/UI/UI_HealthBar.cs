using UnityEngine;
using UnityEngine.UIElements;

public class UI_HealthBar : MonoBehaviour {
    private VisualElement _HBforeground;
    private int _lentgh;

    private void OnEnable() {
        Health.OnDamageTaken += Health_OnDamageTaken;
    }

    private void OnDisable() {
        Health.OnDamageTaken -= Health_OnDamageTaken;
    }

    private void Start() {
        _lentgh = 100;
        _HBforeground = GameManager.Instance.UIManager.Root.Q("HBFore");
        UpdateHealthBar();
    }

    private void Health_OnDamageTaken(int value){
        _lentgh -= value;
        if(_lentgh <= 0){
            _lentgh = 0;
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar(){
        _HBforeground.style.width = Length.Percent(_lentgh);
    }
}