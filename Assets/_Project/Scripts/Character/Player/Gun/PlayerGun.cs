using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    public static Action<int, int, int> OnAmmoCountChange;
    public static Action<PlayerGun, int> OnWeaponChange;
    public static Action<SoundSO> OnShootFired;
    public static Action<SoundSO> OnGunReload;
    public static Action<Vector3> OnShootHit;

    [SerializeField] private Gun _activeGun;
    [SerializeField] private Transform _activeGunTransform;
    private bool _canShoot;
    private IEnumerator _shotRoutine;

    private Player _player;
    private CinemachineImpulseSource _impulseSource;
    private Transform _firstPersonCameraTransform;
    [SerializeField] private FirstPersonCamera _firstPersonCamera;
    
    private int _maxAmmo;
    private int _ammoLeftInMag;
    private Recoil _recoil;
    private bool _resetZoom;
    public bool _resetAimPosition;

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
        _maxAmmo += amount;
        if(_maxAmmo >= _activeGun.Magazine * 3){
            _maxAmmo = _activeGun.Magazine * 3;
        }
    }
    
    private void UpdateAmmoCount(){
        if(_maxAmmo <= 0){
            _maxAmmo = 0;
        }

        OnAmmoCountChange?.Invoke(_ammoLeftInMag, _activeGun.Magazine, _maxAmmo);
    }

    public void HandleGunReload(){
        if(!_player.Input.Reload){return;}                          //If the reload button was not pressed, return;
        if(_ammoLeftInMag ==_activeGun.Magazine){return;}           //If was pressed but the mag is full, return

        var amountFilled = _activeGun.Magazine - _ammoLeftInMag;

        if(amountFilled > _maxAmmo){
            amountFilled = _maxAmmo;
        }else{
            _maxAmmo -= amountFilled;
        }
        _ammoLeftInMag += amountFilled;

        OnGunReload?.Invoke(_activeGun.ReloadSound);
        UpdateAmmoCount();
    }
    #endregion
    
    #region Shoot
    public void HandleShoot(){
        if(_player.Input.Shoot){
            if(_ammoLeftInMag > 0){
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
        for(int i = 0; i < _ammoLeftInMag; i++){
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

        int layerMask = ~LayerMask.GetMask("NoHitLayer"); // Cria uma máscara para todas as camadas exceto a "NoHitLayer"

        if (Physics.Raycast(_firstPersonCameraTransform.position, _firstPersonCameraTransform.forward, out RaycastHit hit, Mathf.Infinity, layerMask)){
            _activeGun.FirePoint.LookAt(hit.point);
            OnShootHit?.Invoke(hit.point);
        }else{
            _activeGun.FirePoint.LookAt(_firstPersonCameraTransform.position + (_firstPersonCameraTransform.forward * 30f));
        }

        _ammoLeftInMag--;
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
    }

    public void ChangeActiveGun(Gun activeGun){
        _activeGun = activeGun;
        _activeGunTransform = _activeGun.GetComponent<Transform>();
        _ammoLeftInMag = _activeGun.Magazine;
        _maxAmmo = _activeGun.Magazine * 3;
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