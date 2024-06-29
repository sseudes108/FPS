using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    public static Action<int, int, int> OnAmmoCountChange;
    public static Action<SoundSO> OnShootFired;
    public static Action<PlayerGun, int> OnWeaponChange;

    private Gun _activeGun;
    private bool _canShoot;
    private IEnumerator _shotRoutine;

    private Player _player;
    private CinemachineImpulseSource _impulseSource;
    private Transform _firstPersonCameraTransform;
    private FirstPersonCamera _firstPersonCamera;
    
    private int _maxAmmo;
    private int _ammoLeftInMag;
    private Recoil _recoil;
    private bool _resetZoom;

    private void Awake() {
        _player = GetComponent<Player>();
        _recoil = transform.Find("Model/CameraRecoil/").GetComponent<Recoil>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _firstPersonCamera = _player.Camera.GetCameraTransform().GetComponent<FirstPersonCamera>();
    }

    public void Start() {
        OnWeaponChange?.Invoke(this, 0);
        ReloadGun();
    }
    
    public void HandleAim(){
        if(_player.Input.Aim){
            _resetZoom = true;
            _firstPersonCamera.ZoomIn(_activeGun.ZoomAmount);
        }else if(_resetZoom){
            _resetZoom = false;
            _firstPersonCamera.ZoomOut();
        }
    }

    public void HandleSwitchGun(){
        if(_player.Input.Previous){
            OnWeaponChange?.Invoke(this, -1);
        }else if(_player.Input.Next){
            OnWeaponChange?.Invoke(this, +1);
        }
    }

    public void ChangeActiveGun(Gun activeGun){
        _activeGun = activeGun;
        _ammoLeftInMag = _activeGun.Magazine;
        _maxAmmo = _activeGun.Magazine * 3;
    }

    public void HandleShoot(){
        if(_player.Input.Shoot){
            if(_ammoLeftInMag > 0){
                if(_activeGun.CanAutoFire){
                    if(_shotRoutine == null){
                        _shotRoutine = ShootRoutine();
                        StartCoroutine(_shotRoutine);
                    }
                }else{
                    if(!_canShoot){
                        return;
                    }else{
                        InstantiateProjectile();;
                        _canShoot = false;
                    }
                }
            }else{
                Debug.Log($"No Ammo In Mag");
            }
        }else{
            _canShoot = true;
            if(_shotRoutine != null){
                StopCoroutine(_shotRoutine);
                _shotRoutine = null;
            }
        }
    }

    private IEnumerator ShootRoutine(){
        float firerate = _activeGun.Firerate;
        for(int i = 0; i < _ammoLeftInMag; i++){
            InstantiateProjectile();
            yield return new WaitForSeconds(firerate);
        }
        _shotRoutine = null;
        _canShoot = false;
    }

    private void InstantiateProjectile(){
        if (_firstPersonCameraTransform == null){
            _firstPersonCameraTransform = _player.Camera.GetCameraTransform();
        }

        int layerMask = ~LayerMask.GetMask("NoHitLayer"); // Cria uma máscara para todas as camadas exceto a "NoHitLayer"

        if (Physics.Raycast(_firstPersonCameraTransform.position, _firstPersonCameraTransform.forward, out RaycastHit hit, Mathf.Infinity, layerMask)){
            _activeGun.GetFirePoint().LookAt(hit.point);
        }else{
            _activeGun.GetFirePoint().LookAt(_firstPersonCameraTransform.position + (_firstPersonCameraTransform.forward * 30f));
        }

        _ammoLeftInMag--;
        ShootFired(hit.point);
    }

    private void ShootFired(Vector3 hitPoint){
        OnShootFired?.Invoke(_activeGun.ShootSound);
        
        _activeGun.Shoot();
        _recoil.RecoilFire();

        UpdateAmmoCount();
        ShakeCamera();
    }

    private void UpdateAmmoCount(){
        if(_maxAmmo <= 0){
            _maxAmmo = 0;
        }
        OnAmmoCountChange?.Invoke(_ammoLeftInMag, _activeGun.Magazine, _maxAmmo);
    }

    public void ReloadGun(){
        var amountFilled = _activeGun.Magazine - _ammoLeftInMag;

        if(amountFilled > _maxAmmo){
            amountFilled = _maxAmmo;
        }else{
            _maxAmmo -= amountFilled;
        }
        _ammoLeftInMag += amountFilled;
        UpdateAmmoCount();
    }

    public void PickUpAmmo(int amount){
        _maxAmmo += amount;
        if(_maxAmmo >= _activeGun.Magazine * 3){
            _maxAmmo = _activeGun.Magazine * 3;
        }
    }

    private void ShakeCamera(){
        _impulseSource.GenerateImpulseWithForce(_activeGun.RecoilForce);
    }
}