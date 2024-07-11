using UnityEngine;
using UnityEngine.UIElements;

public class UI_HealthBar : MonoBehaviour {
    [field:SerializeField] public HealthEventHandlerSO HealthManager { get; private set;}
    private VisualElement _HBforeground;
    private int _lentgh = 100;

    private void OnEnable() {
        HealthManager.OnHealthChange.AddListener(HealthManager_OnHealthChange);
    }

    private void OnDisable() {
        HealthManager.OnHealthChange.RemoveListener(HealthManager_OnHealthChange);
    }

    private void Start() {
        _HBforeground = GameController.Instance.UIManager.Root.Q("HBFore");
        UpdateHealthBar();
    }

    private void HealthManager_OnHealthChange(int currentHP){
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