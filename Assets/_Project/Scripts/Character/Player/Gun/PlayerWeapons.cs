using System.Collections;
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
                var totalBullets = gun.GunData.Magazine * 3;
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
        if(_ammoAmountDict[_activeGunName] > _weapons[_activeWeaponIndex].GunData.Magazine * 3){
            _ammoAmountDict[_activeGunName] = _weapons[_activeWeaponIndex].GunData.Magazine * 3;
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
            gun.SetIsAiming(false);
            gun.DeactiveGun();
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
            if(pickedGun.GunData.WeaponType == weapon.GunData.WeaponType){
                pickedGun.PickUpGun();
                weapon.SetAvailable();
            }
        }
        CheckGuns();
        UpdateFullAmmoInventory();
    }

    private void PlayerGun_OnWeaponChange(PlayerGun playerGun, int key){
        StartCoroutine(ChangeWeaponRoutine(playerGun, key)); //Coroutine used to be asure that the available guns list has been updated
        
    }

    private IEnumerator ChangeWeaponRoutine(PlayerGun playerGun, int key){
        yield return null;
        if(key == -1){//previous
            _activeWeaponIndex--;

            if(_activeWeaponIndex < 0){//if the index is less than 0
                _activeWeaponIndex = _availableGuns.Count - 1; //go to end of the available guns list
            }            
        }else if(key == 1){//next
            _activeWeaponIndex++;

            if(_activeWeaponIndex > _availableGuns.Count - 1){//if the index is greater than the available list count -1 (2 guns -1 = [0, 1] indexes)
                _activeWeaponIndex = 0;//go to start of the available guns list
            }
        }else{//when start game
            _activeWeaponIndex = 0;
        }

        if(_activeGunName == _availableGuns[_activeWeaponIndex].name){//In the start the pistol is index 0, but when other gun is picked becames 1, so the first change without this correction change index 0 for index 1 both are the pistol, so no visible change.
            _activeWeaponIndex = 0;
        }

        ChangeWeapon(playerGun, _activeWeaponIndex);
    }

#endregion
 
}