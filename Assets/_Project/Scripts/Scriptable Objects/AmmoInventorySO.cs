using UnityEngine;

[CreateAssetMenu(fileName = "AmmoInventorySO", menuName = "FPS/Misc/Ammo Inventory", order = 1)]
public class AmmoInventorySO : ScriptableObject {

    private int _pistolBullets, _riffleBullets, _sniperBullets, _rockets;
    private int _maxPistolBullets = 162, _maxRiffleBullets = 324, _maxSniperBullets = 108, _maxRockets = 9;

    public void UpdateBullets(EWeaponTypes weaponType, int value){
        switch(weaponType){

            case EWeaponTypes.Pistol:
                if(_pistolBullets + value >= _maxPistolBullets){
                    _pistolBullets = _maxPistolBullets;
                }else if(_pistolBullets + value <= 0){
                    _pistolBullets = 0;
                }else{
                    _pistolBullets += value;
                }
            break;

            case EWeaponTypes.AssaultRifle:
                if(_riffleBullets + value >= _maxRiffleBullets){
                    _riffleBullets = _maxRiffleBullets;
                }else if(_riffleBullets + value <= 0){
                    _riffleBullets = 0;
                }else{
                    _riffleBullets += value;
                }
            break;

            case EWeaponTypes.Sniper:
                if(_sniperBullets + value >= _maxSniperBullets){
                    _sniperBullets = _maxSniperBullets;
                }else if(_sniperBullets + value <= 0){
                    _sniperBullets = 0;
                }else{
                    _sniperBullets += value;
                }
            break;

            case EWeaponTypes.RocketLauncher:
                if(_rockets + value >= _maxRockets){
                    _rockets = _maxRockets;
                }else if(_rockets + value <= 0){
                    _rockets = 0;
                }else{
                    _rockets += value;
                }
            break;
        }
    }

    public int GetCurrentGunTypeBulletInventoryCount(EWeaponTypes weaponType){
        var currentBulletCount = weaponType switch{
            EWeaponTypes.Pistol => _pistolBullets,
            EWeaponTypes.AssaultRifle => _riffleBullets,
            EWeaponTypes.Sniper => _sniperBullets,
            EWeaponTypes.RocketLauncher => _rockets,
            _ => 0,
        };
        return currentBulletCount;
    }
}