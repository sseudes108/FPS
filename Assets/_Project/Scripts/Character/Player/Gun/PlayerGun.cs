using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerGun : MonoBehaviour {

    //Events
    public static Action<int, int, int> OnAmmoCountChange;
    public static Action<PlayerGun, int> OnWeaponChange;
    public static Action<SoundSO> OnShootFired;
    public static Action<SoundSO> OnGunReload;
    public static Action<Vector3> OnShootHit;

    //Components
    [SerializeField] private Gun _activeGun;
    private PlayerWeapons _weapons;
    private Player _player;
    private Recoil _recoil;

    //Camera and Recoil
    private Transform _firstPersonCameraTransform;
    [SerializeField] private FirstPersonCamera _firstPersonCamera;
    private CinemachineImpulseSource _impulseSource;
    private bool _resetZoom;

    //Shoot
    private IEnumerator _shotRoutine;
    private bool _canShoot;
    private bool _isReloading = false;
   

#region UnityMethods

    private void OnEnable() {
        GameManager.OnGamePaused += GameManager_OnGamePaused;
    }

    private void OnDisable() {
        GameManager.OnGamePaused -= GameManager_OnGamePaused;
        
    }

    private void Awake() {
        _player = GetComponent<Player>();
        _recoil = transform.Find("Model/CameraRecoil/").GetComponent<Recoil>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _weapons = GetComponent<PlayerWeapons>();
    }

    public void Start() {
        OnWeaponChange?.Invoke(this, 0);
        HandleGunReload();
    }
#endregion

#region Custom Methods

    #region Aim

    public void HandleAim(){
        if(_player.Input.Aim){
            _resetZoom = true;
            _activeGun.SetIsAiming(true);
            _firstPersonCamera.ZoomIn(_activeGun.ZoomAmount);
        }else if(_resetZoom){
            _activeGun.SetIsAiming(false);
            _resetZoom = false;
            _firstPersonCamera.ZoomOut();
        }
    }

    #endregion

    #region Ammo
    public void PickUpAmmo(int amount){
        _weapons.AddBulletsToInventory(amount);
    }
    
    private void UpdateAmmoCount(){
        OnAmmoCountChange?.Invoke(_activeGun.AmmoLeftInMag, _activeGun.Magazine, _weapons.GetCurrentGunAmmoInventory());
    }

    private IEnumerator ReloadRoutine(int amountToFill){
        yield return new WaitForSeconds(_activeGun.ReloadTime);
        _weapons.RemoveBulletsFromInventory(amountToFill);
        _activeGun.ReloadMagazine(amountToFill);
        UpdateAmmoCount();
        _isReloading = false;
        yield return null;
    }

    public void HandleGunReload(){
        if(!_player.Input.Reload){return;}
        if(_activeGun.AmmoLeftInMag ==_activeGun.Magazine){return;}
        _isReloading = true;

        var bulletsLeftInTotal = _weapons.GetCurrentGunAmmoInventory();
        if(bulletsLeftInTotal == 0){ return;} // if has no bullet left to reload

        var amountToFill = _activeGun.Magazine - _activeGun.AmmoLeftInMag;;

        if(amountToFill > bulletsLeftInTotal){
            amountToFill = bulletsLeftInTotal;
        }

        OnGunReload?.Invoke(_activeGun.ReloadSound);
        StartCoroutine(ReloadRoutine(amountToFill));
    }
    #endregion
    
    #region Shoot
    public void HandleShoot(){
        if(_isReloading){return;}
        if(_player.Input.Shoot){
            if(_activeGun.AmmoLeftInMag > 0){
                if(_activeGun.CanAutoFire){
                    if(_shotRoutine == null){
                        _shotRoutine = AutomaticFireRoutine();
                        StartCoroutine(_shotRoutine);
                    }
                }else{
                    if(!_canShoot){
                        return;
                    }else{
                        ShootProjectile();
                        _canShoot = false;
                    }
                }
            }else{
                //Play Some SFX To indicate no ammo left
            }
        }else{
            _canShoot = true;
            if(_shotRoutine != null){
                StopCoroutine(_shotRoutine);
                _shotRoutine = null;
            }
        }
    }

    private IEnumerator AutomaticFireRoutine(){
        float firerate = _activeGun.Firerate;
        for(int i = 0; i < _activeGun.AmmoLeftInMag; i++){
            ShootProjectile();
            yield return new WaitForSeconds(firerate);
        }
        _shotRoutine = null;
        _canShoot = false;
    }

    private void ShootProjectile(){
        if (_firstPersonCameraTransform == null){
            _firstPersonCameraTransform = _player.Camera.GetCameraTransform();
        }

        int layerMask = ~LayerMask.GetMask("NoHit"); // Cria uma máscara para todas as camadas exceto a "NoHitLayer"

        if (Physics.Raycast(_firstPersonCameraTransform.position, _firstPersonCameraTransform.forward, out RaycastHit hit, Mathf.Infinity, layerMask)){
            _activeGun.FirePoint.LookAt(hit.point);
            OnShootHit?.Invoke(hit.point);
        }else{
            _activeGun.FirePoint.LookAt(_firstPersonCameraTransform.position + (_firstPersonCameraTransform.forward * 30f));
        }

        ShootFired();
    }

    private void ShootFired(){
        OnShootFired?.Invoke(_activeGun.ShootSound);
        
        _activeGun.Shoot();
        _recoil.RecoilFire();

        UpdateAmmoCount();
        _impulseSource.GenerateImpulseWithForce(_activeGun.RecoilForce);
    }

    #endregion

    #region Gun Settings
    public void HandleSwitchGun(){
        if(_player.Input.Previous){
            OnWeaponChange?.Invoke(this, -1);
        }else if(_player.Input.Next){
            OnWeaponChange?.Invoke(this, +1);
        }
        UpdateAmmoCount();
    }

    public void ChangeActiveGun(Gun activeGun){
        _activeGun = activeGun;
    }
    #endregion

#endregion

#region Events

    private void GameManager_OnGamePaused(bool paused){
        StartCoroutine(UpdateAmmoRoutine());
    }
    private IEnumerator UpdateAmmoRoutine(){
        yield return new WaitForEndOfFrame();
        UpdateAmmoCount();
        yield return null;
    }

#endregion

}