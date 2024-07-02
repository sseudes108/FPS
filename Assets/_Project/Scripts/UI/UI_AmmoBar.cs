using UnityEngine;
using UnityEngine.UIElements;

public class UI_AmmoBar : MonoBehaviour {
    private Label _ammoLabel;
    private Label _ammoMaxLabel;

    private void OnEnable() {
        PlayerGun.OnAmmoCountChange += PlayerGun_OnAmmoCountChange;
    }

    private void OnDisable() {
        PlayerGun.OnAmmoCountChange -= PlayerGun_OnAmmoCountChange;
    }

    private void Start() {
        _ammoLabel = GameManager.Instance.UIManager.Root.Q<Label>("AmmoLabel");
        _ammoMaxLabel = GameManager.Instance.UIManager.Root.Q<Label>("AmmoMaxLabel");
    }

    private void PlayerGun_OnAmmoCountChange(int ammoLeft, int magSize, int maxAmmo){
        _ammoLabel.text = $"{ammoLeft}/{magSize}";
        _ammoMaxLabel.text = $"{maxAmmo}";
    }
}