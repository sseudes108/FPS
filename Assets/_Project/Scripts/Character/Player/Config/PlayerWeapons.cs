using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour {
    [SerializeField] public List<Gun> Weapons = new();
    public MachineGun MachineGun {get; private set;}
    public Pistol Pistol {get; private set;}
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
            if(nextIndex >= Weapons.Count -1){
                _activeWeaponIndex = 0;
            }
        }else if(key == -1){
            var nextIndex = _activeWeaponIndex--;
            if(nextIndex <= 0){
                _activeWeaponIndex = Weapons.Count -1;
            }
        }else{
            int random = Random.Range(0, Weapons.Count);
            _activeWeaponIndex = random;
        }

        ChangeWeapon(playerGun, _activeWeaponIndex);
    }

    private void ChangeWeapon(PlayerGun playerGun, int index){
        foreach(var gun in Weapons){
            gun.gameObject.SetActive(false);
        }
        Weapons[index].gameObject.SetActive(true);
        playerGun.ChangeActiveGun(Weapons[index]);
    }
}