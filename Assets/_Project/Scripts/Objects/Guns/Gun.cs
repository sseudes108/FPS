using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour{
    public static Action<Gun> OnPlayerCloseForPickUp;
    public static Action<bool> OnPlayerMoveOutRange;
    
    protected ObjectPool<Bullet> _bulletPool;
    protected Character _character;

    [SerializeField] protected GunSO _gunData;
    public GunSO GunData => _gunData;
    
    [SerializeField] private bool _isAvailable;
    public bool IsAvailable => _isAvailable;

    private int _ammoLeftInMag;
    public int AmmoLeftInMag => _ammoLeftInMag;
    protected Transform _firePoint;
    public Transform FirePoint => _firePoint;
    protected Transform _muzzleFlash;
    private Transform _hip;
    private Transform _aim;
    private Transform _model;
    private bool _isAiming;
    public bool IsAiming => _isAiming;
    private bool _playerInRange = true;

#region UnityMethods

    private void Awake() {    
        CreateBulletPool();
        _character = GetComponent<Character>();
        _hip = transform.Find("States/Hip");
        _aim = transform.Find("States/Aim");
        _model = transform.Find("Model");
        _firePoint = transform.Find("Model/FirePoint");
        _muzzleFlash = transform.Find("Model/FirePoint/MuzzleFlash");
    }

    private void Start() {
        ReloadMagazine(_gunData.Magazine);
        // ReloadMagazine(Magazine);
    }

    private void Update() { 
        Aim();
        DetectPlayer();
    }
#endregion

#region Custom Methods

    private IEnumerator MuzzleFlashRoutine(){
        _muzzleFlash.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        _muzzleFlash.gameObject.SetActive(false);
    }

    public void Shoot(){
        _ammoLeftInMag--;
        StartCoroutine(MuzzleFlashRoutine());
        var newBullet = _bulletPool.Get();
        newBullet.Init(this, _gunData.BulletMaterial, _gunData.DamageValue, _character, _firePoint);
        // newBullet.Init(this, _bulletMaterial, _damageValue, _character, _firePoint);
    }

    public void Aim(){
        // if(!_gunData.IsAvailable){return;};
        // if(_gunData.IsAiming){
        //     _gunData.Model.position = Vector3.Lerp(_gunData.Model.position, _gunData.Aim.position, _gunData.AimSpeed * Time.deltaTime);
        // }else{
        //     _gunData.Model.position = Vector3.Lerp(_gunData.Model.position, _gunData.Hip.position, _gunData.AimSpeed * Time.deltaTime);
        // }
        if(!_isAvailable){return;};
        if(_isAiming){
            _model.position = Vector3.Lerp(_model.position, _aim.position, _gunData.AimSpeed * Time.deltaTime);
        }else{
            _model.position = Vector3.Lerp(_model.position, _hip.position, _gunData.AimSpeed * Time.deltaTime);
        }
    }

    public void SetIsAiming(bool isAiming){
        _isAiming = isAiming;
    }

    public void ReloadMagazine(int amount){
        _ammoLeftInMag += amount;
        if(_ammoLeftInMag > _gunData.Magazine){
            _ammoLeftInMag = _gunData.Magazine;
        }
    }
    
    private void CreateBulletPool(){
        _bulletPool = new ObjectPool<Bullet>(()=>{
            return Instantiate(_gunData.BulletPrefab);
            // return Instantiate(_bulletPrefab);
        }, newBullet =>{
            newBullet.gameObject.SetActive(true);
        }, newBullet =>{
            newBullet.gameObject.SetActive(false);
        }, newBullet =>{
            Destroy(newBullet);
        }, false, 50, 70);
    }

    public void ReleaseFromPool(Bullet bullet){
        _bulletPool.Release(bullet);
    }

    // private void OnDrawGizmos() {
    //     if(!_gunData.IsAvailable){
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawSphere(transform.position, 1.5f);
    //     }
    //     // if(!IsAvailable){
    //     //     Gizmos.color = Color.yellow;
    //     //     Gizmos.DrawSphere(transform.position, 1.5f);
    //     // }
    // }

    private void DetectPlayer(){
        if(_isAvailable){
            return;
        }

        Collider[] player = new Collider[1];
        var playerInRange = Physics.OverlapSphereNonAlloc(transform.position, 1.5f, player, LayerMask.GetMask("Player"));

        if(playerInRange > 0 && player[0] != null){
            _playerInRange = true;
            OnPlayerCloseForPickUp?.Invoke(this);
        }else{
            if(!_playerInRange){return;}
            _playerInRange = false;
            OnPlayerMoveOutRange?.Invoke(false);
        }
    }

    public void PickUpGun(){
        gameObject.SetActive(false);
        Destroy(gameObject, 5f);
        OnPlayerMoveOutRange?.Invoke(false);
    }

    public void SetAvailable(){
        _isAvailable = true;
    }

#endregion

#region Events

#endregion
}