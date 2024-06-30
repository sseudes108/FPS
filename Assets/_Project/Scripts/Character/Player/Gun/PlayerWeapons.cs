using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
    [SerializeField] private List<Gun> _weapons = new();
    public int _activeWeaponIndex;

    private void OnEnable() {
        PlayerGun.OnWeaponChange += PlayerGun_OnWeaponChange;
    }
    private void OnDisable() {
        PlayerGun.OnWeaponChange -= PlayerGun_OnWeaponChange;
    }

    private void PlayerGun_OnWeaponChange(PlayerGun playerGun, int key){
        if(key == 1){
            var nextIndex = _activeWeaponIndex++;
            if(nextIndex >= _weapons.Count -1){
                _activeWeaponIndex = 0;
            }
        }else if(key == -1){
            var nextIndex = _activeWeaponIndex--;
            if(nextIndex <= 0){
                _activeWeaponIndex = _weapons.Count -1;
            }
        }else{
            _activeWeaponIndex = 0;
        }
        
        ChangeWeapon(playerGun, _activeWeaponIndex);
    }

    private void ChangeWeapon(PlayerGun playerGun, int index){
        foreach(var gun in _weapons){
            gun.gameObject.SetActive(false);
        }
        _weapons[index].gameObject.SetActive(true);
        playerGun.ChangeActiveGun(_weapons[index]);
    }
}