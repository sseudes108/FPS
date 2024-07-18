using UnityEngine;
using UnityEngine.UIElements;

public class UI_HealthBar : MonoBehaviour {
    [field:SerializeField] public HealthManagerSO HealthManager { get; private set;}
    private VisualElement _HBforeground;
    private int _lenght = 100;
    
    private void OnEnable() {
        HealthManager.OnHealthChange.AddListener(HealthManager_OnHealthChange);
    }

    private void OnDisable() {
        HealthManager.OnHealthChange.RemoveListener(HealthManager_OnHealthChange);
    }

    private void Start() {
        UpdateHealthBar();
    }

    private void HealthManager_OnHealthChange(int currentHP){
        _lenght = currentHP;
        if (_lenght <= 0){
            _lenght = 0;
        }
        UpdateHealthBar();
    }

    public void UpdateHealthBar(){
        _HBforeground = GameController.Instance.UIManager.Root.Q("HBFore");
        _HBforeground.style.width = Length.Percent(_lenght);
    }
}