using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerGun : MonoBehaviour {
    [field:SerializeField] public AudioManagerSO AudioManager  { get; private set;}
    [field:SerializeField] public GunManagerSO GunManager  { get; private set;}
    [field:SerializeField] public GameManagerSO GameManager { get; private set;}

    //Components
    private Gun _activeGun;
    private PlayerWeapons _weapons;
    private Player _player;
    private Recoil _recoil;

    //Camera and Recoil
    private Transform _firstPersonCameraTransform;
    private FirstPersonCamera _firstPersonCamera;
    private CinemachineImpulseSource _impulseSource;
    private bool _resetZoom;

    //Shoot
    private IEnumerator _shotRoutine;
    private bool _canShoot;
    private bool _isReloading = false;

   
#region UnityMethods

    private void OnEnable() {
        GameManager.OnGamePaused.AddListener(GameManager_OnGamePaused);
    }

    private void OnDisable() {
        GameManager.OnGamePaused.RemoveListener(GameManager_OnGamePaused);
    }

    private void Awake() {
        _player = GetComponent<Player>();
        _recoil = transform.Find("Model/Camera/").GetComponent<Recoil>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        _weapons = GetComponent<PlayerWeapons>();
        _firstPersonCameraTransform = transform.Find("Model/Camera/FPS Cam");
        _firstPersonCamera = _firstPersonCameraTransform.GetComponent<FirstPersonCamera>();
    }

    public void Start() {
        GunManager.OnWeaponChange?.Invoke(this, 0);
        HandleGunReload();
    }
#endregion

#region Custom Methods

    #region Aim

    public void HandleAim(){
        if(_player.Input.Aim){
            _resetZoom = true;
            _activeGun.SetIsAiming(true);
            _firstPersonCamera.ZoomIn(_activeGun.GunData.ZoomAmount);
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
        if(_activeGun != null){
            GunManager.OnAmmoCountChange?.Invoke(_activeGun.AmmoLeftInMag, _activeGun.GunData.Magazine, _weapons.GetCurrentGunAmmoInventory());
        }
    }

    private IEnumerator ReloadRoutine(int amountToFill){
        yield return new WaitForSeconds(_activeGun.GunData.ReloadTime);
        _weapons.RemoveBulletsFromInventory(amountToFill);
        _activeGun.ReloadMagazine(amountToFill);
        UpdateAmmoCount();
        _isReloading = false;
        yield return null;
    }

    public void HandleGunReload(){
        if(!_player.Input.Reload){return;}
        if(_activeGun.AmmoLeftInMag ==_activeGun.GunData.Magazine){return;}

        var bulletsLeftInTotal = _weapons.GetCurrentGunAmmoInventory();
        if(bulletsLeftInTotal == 0){ return;} // if has no bullet left to reload
        
        _isReloading = true;

        var amountToFill = _activeGun.GunData.Magazine - _activeGun.AmmoLeftInMag;;

        if(amountToFill > bulletsLeftInTotal){
            amountToFill = bulletsLeftInTotal;
        }

        AudioManager.PlayReloadSound(this, _activeGun.GunData.ReloadSound);
        StartCoroutine(ReloadRoutine(amountToFill));
    }
    #endregion
    
    #region Shoot
    public void HandleShoot(){
        if(_isReloading){return;}

        if(_player.Input.Shoot){//if player is shooting
            if(_activeGun.AmmoLeftInMag > 0){//if has bullet to spend
                if(_activeGun.GunData.CanAutoFire){//if the gun has autofire
                    if(_shotRoutine == null){
                        _shotRoutine = AutomaticFireRoutine();
                        StartCoroutine(_shotRoutine);
                    }
                }else{//if has no autofire
                    if(!_canShoot){//if can't shoo
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
        float firerate = _activeGun.GunData.Firerate;
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
        }else{
            _activeGun.FirePoint.LookAt(_firstPersonCameraTransform.position + (_firstPersonCameraTransform.forward * 30f));
        }

        ShootFired();
    }

    private void ShootFired(){
        AudioManager.PlayShootSound(this, _activeGun.GunData.ShootSound);
        
        _activeGun.Shoot();
        _recoil.RecoilFire();

        UpdateAmmoCount();
        _impulseSource.GenerateImpulseWithForce(_activeGun.GunData.RecoilForce);
    }

    #endregion

    #region Gun Settings
    public void HandleSwitchGun(){
        if(_player.Input.Previous){
            GunManager.OnWeaponChange?.Invoke(this, -1);
            AudioManager.PlayHandleGunSound(this, _activeGun.GunData.HandlingSound);
        }else if(_player.Input.Next){
            GunManager.OnWeaponChange?.Invoke(this, +1);
            AudioManager.PlayHandleGunSound(this, _activeGun.GunData.HandlingSound);
        }
        UpdateAmmoCount();
    }

    public void ChangeActiveGun(Gun activeGun){
        _activeGun = activeGun;
        activeGun.ActiveGun();
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