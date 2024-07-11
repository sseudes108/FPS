using UnityEngine;
using UnityEngine.UIElements;

public class UI_AmmoBar : MonoBehaviour {
    [SerializeField] private GunEventHandlerSO GunManager;
    private Label _ammoLabel;
    private Label _ammoMaxLabel;

    private void OnEnable() {
        GunManager.OnAmmoCountChange.AddListener(PlayerGun_OnAmmoCountChange);
    }

    private void OnDisable() {
        GunManager.OnAmmoCountChange.RemoveListener(PlayerGun_OnAmmoCountChange);
    }

    private void Start() {
        _ammoLabel = GameController.Instance.UIManager.Root.Q<Label>("AmmoLabel");
        _ammoMaxLabel = GameController.Instance.UIManager.Root.Q<Label>("AmmoMaxLabel");
    }

    private void PlayerGun_OnAmmoCountChange(int ammoLeft, int magSize, int maxAmmo){
        _ammoLabel.text = $"{ammoLeft}/{magSize}";
        _ammoMaxLabel.text = $"{maxAmmo}";
    }
}