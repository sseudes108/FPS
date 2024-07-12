using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Sway))]
public class Gun : MonoBehaviour{
    [SerializeField] private GunEventHandlerSO GunManager;
    
    protected Character _character;

    [SerializeField] protected GunSO _gunData;
    public GunSO GunData => _gunData;
    
    [SerializeField] private bool _isAvailable;
    public bool IsAvailable => _isAvailable;


    // *** SET PRIVATE *** ///
    public int _ammoLeftInMag;
    public int AmmoLeftInMag => _ammoLeftInMag;
    [SerializeField] protected Transform _firePoint;
    public Transform FirePoint => _firePoint;
    private Transform _muzzleFlash;
    private Transform _hip;
    private Transform _aim;
    private Transform _model;
    private bool _isAiming;
    public bool IsAiming => _isAiming;
    private bool _playerInRange = true;
    private bool _isActive = false;

#region UnityMethods

    private void Awake() {    
        _character = GetComponent<Character>();
        _hip = transform.Find("States/Hip");
        _aim = transform.Find("States/Aim");
        _model = transform.Find("Model");
        _firePoint = transform.Find("Model/FirePoint");
        _muzzleFlash = transform.Find("Model/FirePoint/MuzzleFlash");
    }

    private void Start() {
        ReloadMagazine(_gunData.Magazine);
    }

    private void Update() { 
        DetectPlayer();
    }

    private void FixedUpdate() { 
       Aim();
    }
#endregion

#region Custom Methods
    #region Shoot

    public void Shoot(){
        StartCoroutine(ShootRoutine());
    }

    public IEnumerator ShootRoutine(){
        _ammoLeftInMag--;
        StartCoroutine(MuzzleFlashRoutine());
        yield return null;

        GunManager.SetFirePosition(FirePoint.position);

        Bullet newBullet;
        do{
            newBullet = GunManager.BulletPool.Get();
        }while(newBullet == null);
        
        newBullet.transform.SetPositionAndRotation(FirePoint.position, Quaternion.identity);
        newBullet.Init(this, _gunData.BulletMaterial, _gunData.DamageValue, _character, FirePoint);
    }

    private IEnumerator MuzzleFlashRoutine(){
        _muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        _muzzleFlash.gameObject.SetActive(false);
    }

    public void ReleaseBulletFromPool(Bullet bullet){
        GunManager.ReleaseBulletFromPool(bullet);
    }

    #endregion
    
    #region Interaction

    private void DetectPlayer(){
        if(_isAvailable){
            return;
        }

        Collider[] player = new Collider[1];
        var playerInRange = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, player, LayerMask.GetMask("Player"));

        if(playerInRange > 0 && player[0] != null){
            _playerInRange = true;
            GunManager.PlayerClosePickUp(this);
        }else{
            if(!_playerInRange){return;}
            _playerInRange = false;
            GunManager.OnPlayerMoveOutRange?.Invoke(false);

        }
    }

    public void PickUpGun(){
        gameObject.SetActive(false);
        Destroy(gameObject, 5f);
        GunManager.OnPlayerMoveOutRange?.Invoke(false);
    }

    #endregion

    #region Aim

    public void Aim(){
        if(!_isAvailable || !_isActive){return;};
        if(_isAiming){
            _model.position = Vector3.Lerp(_model.position, _aim.position, _gunData.AimSpeed * Time.deltaTime);
        }else{
            _model.position = Vector3.Lerp(_model.position, _hip.position, _gunData.AimSpeed * Time.deltaTime);
        }
    }

    public void SetIsAiming(bool isAiming){
        _isAiming = isAiming;
    }

    #endregion
    
    #region Settings

    public void ReloadMagazine(int amount){
        _ammoLeftInMag += amount;
        if(_ammoLeftInMag > _gunData.Magazine){
            _ammoLeftInMag = _gunData.Magazine;
        }
    }

    public void SetAvailable(){_isAvailable = true;}
    public void ActiveGun(){_isActive = true;}
    public void DeactiveGun(){ _isActive = false;}

    #endregion

#endregion
}