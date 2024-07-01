using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
    [SerializeField] private List<Gun> _weapons = new();
    private Dictionary<string, int> _ammoAmountDict = new();
    private List<Gun> _availableGuns = new();

    private int _activeWeaponIndex;
    private string _activeGunName;


#region UnityMethods

    private void OnEnable() {
        PlayerGun.OnWeaponChange += PlayerGun_OnWeaponChange;
        Player.OnWeaponPickUp += Player_OnWeaponPickUp;
    }
    
    private void OnDisable() {
        PlayerGun.OnWeaponChange -= PlayerGun_OnWeaponChange;
        Player.OnWeaponPickUp -= Player_OnWeaponPickUp;
    }
    public void Start(){
        CheckGuns();
        UpdateFullAmmoInventory();
    }

#endregion

#region Custom Methods

    #region Ammo Inventory
    private void UpdateFullAmmoInventory(){
        foreach(var gun in _availableGuns){
            if(!_ammoAmountDict.ContainsKey(gun.name)){ //If the actual gun does not exist in the dict, so the ammo count still the same already saved
                var totalBullets = gun.Magazine * 3;
                _ammoAmountDict.Add(gun.name, totalBullets);
            }
        }
    }

    public int GetCurrentGunAmmoInventory(){
        return _ammoAmountDict[_activeGunName]; //Used in the UI to show the actual ammo amount of the active gun
    }

    public void RemoveBulletsFromInventory(int bulletsUsed){
        _ammoAmountDict[_activeGunName] -= bulletsUsed;
    }

    public void AddBulletsToInventory(int bulletsGained){
        _ammoAmountDict[_activeGunName] += bulletsGained;
        if(_ammoAmountDict[_activeGunName] > _weapons[_activeWeaponIndex].Magazine * 3){
            _ammoAmountDict[_activeGunName] = _weapons[_activeWeaponIndex].Magazine * 3;
        }
    }
    #endregion

    #region Gun
    private void CheckGuns(){
        _availableGuns.Clear();
        foreach (Gun gun in _weapons){
            if(gun.IsAvailable){
                _availableGuns.Add(gun);
            }
        }
    }
    private void ChangeWeapon(PlayerGun playerGun, int index){
        foreach(var gun in _weapons){
            gun.gameObject.SetActive(false);
        }
        _availableGuns[index].gameObject.SetActive(true);
        _activeGunName = _availableGuns[_activeWeaponIndex].name;
        playerGun.ChangeActiveGun(_availableGuns[index]);
    }
    #endregion

#endregion

#region Events

    private void Player_OnWeaponPickUp(Gun pickedGun){
        foreach(var weapon in _weapons){
            if(pickedGun.WeaponType == weapon.WeaponType){
                pickedGun.PickUpGun();
                weapon.SetAvailable();
            }
        }
        CheckGuns();
        UpdateFullAmmoInventory();
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
            _activeWeaponIndex = 0;
        }
        
        ChangeWeapon(playerGun, _activeWeaponIndex);
    }

#endregion
 
}