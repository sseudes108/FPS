using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerWeapons : MonoBehaviour {
    [SerializeField] private List<Gun> _weapons = new();
    [SerializeField] private List<int> _ammoInventory;
    public int _activeWeaponIndex;

    private void OnEnable() {
        PlayerGun.OnWeaponChange += PlayerGun_OnWeaponChange;
    }
    private void OnDisable() {
        PlayerGun.OnWeaponChange -= PlayerGun_OnWeaponChange;
    }

    public void Start(){
        UpdateFullAmmoInventory();
    }

    public void UpdateActiveGunInventory(int bulletsUsed){
        _ammoInventory[_activeWeaponIndex] -= bulletsUsed;
    }

    private void UpdateFullAmmoInventory(){
        foreach(var gun in _weapons){
            var totalBullets = gun.Magazine * 3;
            gun.ReloadMagazine(totalBullets / 3);
            _ammoInventory.Add(totalBullets);
        }
    }

    public int GetCurrentGunAmmoInventory(){
        return _ammoInventory[_activeWeaponIndex];
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