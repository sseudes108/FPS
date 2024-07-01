using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
    [SerializeField] private List<Gun> _weapons = new();
    [SerializeField] private List<int> _ammoInventory;
    public int _activeWeaponIndex;
    [SerializeField] private List<Gun> _availableGuns;

    private void OnEnable() {
        PlayerGun.OnWeaponChange += PlayerGun_OnWeaponChange;
    }
    private void OnDisable() {
        PlayerGun.OnWeaponChange -= PlayerGun_OnWeaponChange;
    }

    public void Start(){
        CheckGuns();
        UpdateFullAmmoInventory();
    }

    private void CheckGuns(){
        foreach (Gun gun in _weapons){
            if(gun.IsAvailable){
                _availableGuns.Add(gun);
            }
        }
    }

    public void RemoveBulletsActiveGun(int bulletsUsed){
        _ammoInventory[_activeWeaponIndex] -= bulletsUsed;
    }

    public void AddBulletsActiveGun(int bulletsGained){
        _ammoInventory[_activeWeaponIndex] += bulletsGained;
        if(_ammoInventory[_activeWeaponIndex] > _weapons[_activeWeaponIndex].Magazine * 3){
            _ammoInventory[_activeWeaponIndex] = _weapons[_activeWeaponIndex].Magazine * 3;
        }
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
            if(nextIndex >= _availableGuns.Count -1){
                _activeWeaponIndex = 0;
            }
        }else if(key == -1){
            var nextIndex = _activeWeaponIndex--;
            if(nextIndex <= 0){
                _activeWeaponIndex = _availableGuns.Count -1;
            }
        }else{
            _activeWeaponIndex = 1;
        }
        
        ChangeWeapon(playerGun, _activeWeaponIndex);
    }

    private void ChangeWeapon(PlayerGun playerGun, int index){
        foreach(var gun in _weapons){
            gun.gameObject.SetActive(false);
        }
        _availableGuns[index].gameObject.SetActive(true);
        playerGun.ChangeActiveGun(_availableGuns[index]);
    }
}