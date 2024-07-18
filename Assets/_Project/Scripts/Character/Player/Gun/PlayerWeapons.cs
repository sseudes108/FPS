using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
    [SerializeField] private GunManagerSO GunManager;
    [SerializeField] private AmmoInventorySO AmmoInventory;

    private List<Gun> _weapons = new();
    private List<Gun> _availableGuns = new();
    private int _activeWeaponIndex;
    private Gun _activeGun;

#region UnityMethods

    private void OnEnable() {
        GunManager.OnWeaponPickUp.AddListener(GunManager_OnWeaponPickUp);
        GunManager.OnWeaponChange.AddListener(GunManager_OnWeaponChange);
    }
    
    private void OnDisable() {
        GunManager.OnWeaponPickUp.RemoveListener(GunManager_OnWeaponPickUp);
        GunManager.OnWeaponChange.RemoveListener(GunManager_OnWeaponChange);
    }

    private void Awake() {
        var gunsArray = transform.Find("Model/Camera/CamTarget/GunHolder").GetComponentsInChildren<Gun>(true);
        foreach(var gun in gunsArray) {
            _weapons.Add(gun);
        }
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
            AmmoInventory.UpdateBullets(gun.GunData.WeaponType, gun.GunData.Magazine * 9);
        }
    }

    public int GetCurrentGunAmmoInventory(){
        return AmmoInventory.GetCurrentGunTypeBulletInventoryCount(_activeGun.GunData.WeaponType);
    }

    public void RemoveBulletsFromInventory(int bulletsUsed){
        AmmoInventory.UpdateBullets(_activeGun.GunData.WeaponType, -bulletsUsed); //negative received value
    }

    public void AddBulletsToInventory(int bulletsGained){
        AmmoInventory.UpdateBullets(_activeGun.GunData.WeaponType, bulletsGained);
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

        _activeGun = _availableGuns[index];
        _activeGun.gameObject.SetActive(true);
        playerGun.ChangeActiveGun(_activeGun);
    }
    #endregion

#endregion

#region Events

    private void GunManager_OnWeaponPickUp(Gun pickedGun){
        foreach(var weapon in _weapons){
            if(pickedGun.GunData.WeaponType == weapon.GunData.WeaponType){
                pickedGun.PickUpGun();
                weapon.SetAvailable();
                AmmoInventory.UpdateBullets(pickedGun.GunData.WeaponType, pickedGun.GunData.Magazine * 9);
            }
        }
        CheckGuns();
    }

    private void GunManager_OnWeaponChange(PlayerGun playerGun, int key){
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
        ChangeWeapon(playerGun, _activeWeaponIndex);
    }

#endregion
 
}