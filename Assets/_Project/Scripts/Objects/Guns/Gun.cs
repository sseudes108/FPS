using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Gun : MonoBehaviour{

    public enum WeaponTypes{
        Pistol = 0,
        Rifle = 1,
        MachineGun = 2,
        RocketLauncher = 3
    }

    protected ObjectPool<Bullet> _bulletPool;
    protected Character _character;
    
    [Header("Settings")]
    [SerializeField] protected WeaponTypes _weaponType;
    [SerializeField] private int _magazine;

    [Header("Fire")]
    protected Transform _firePoint;
    [SerializeField] protected float _firerate;
    [SerializeField] private bool _canAutoFire;
    [SerializeField] protected int _damageValue;
    [SerializeField] protected float _recoilForce;
    [SerializeField] private int _ammoLeftInMag;

    [Header("Bullet")]
    [SerializeField] protected Material _bulletMaterial;
    [SerializeField] protected Bullet _bulletPrefab;
        

    [Header("Aim")]
    [SerializeField] protected float _zoomAmount;
    [SerializeField] private float _aimSpeed;
    private Transform _hip;
    private Transform _aim;
    private Transform _model;
    private bool _isAiming;

    [Header("Sounds")]
    [SerializeField] private SoundSO _shootSound;
    [SerializeField] private SoundSO _reloadSound;

    //Public referencies
    public float ZoomAmount => _zoomAmount;
    public float Firerate => _firerate;
    public float RecoilForce => _recoilForce;
    public bool CanAutoFire => _canAutoFire;
    public int Magazine => _magazine;
    public SoundSO ShootSound => _shootSound;
    public SoundSO ReloadSound => _reloadSound;
    public WeaponTypes WeaponType => _weaponType;
    public Transform FirePoint => _firePoint;
    public float AimSpeed => _aimSpeed;
    public bool IsAiming => _isAiming;
    public int AmmoLeftInMag => _ammoLeftInMag;


#region UnityMethods

    private void Awake() {
        CreateBulletPool();
        _character = GetComponent<Character>();
        _hip = transform.Find("States/Hip");
        _aim = transform.Find("States/Aim");
        _model = transform.Find("Model");
        _firePoint = transform.Find("Model/FirePoint");
    }

    private void Update() { Aim(); }

#endregion

#region Custom Methods

    public void Shoot(){
        _ammoLeftInMag--;
        var newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletMaterial, _damageValue, _character, _firePoint);
    }

    public void Aim(){
        if(_isAiming){
            _model.position = Vector3.Lerp(_model.position, _aim.position, AimSpeed * Time.deltaTime);
        }else{
            _model.position = Vector3.Lerp(_model.position, _hip.position, AimSpeed * Time.deltaTime);
        }
    }

    public void SetIsAiming(bool isAiming){
        _isAiming = isAiming;
    }

    public void ReloadMagazine(int amount){
        _ammoLeftInMag += amount;
    }
    
    private void CreateBulletPool(){
        _bulletPool = new ObjectPool<Bullet>(()=>{
            return Instantiate(_bulletPrefab);
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

#endregion

#region Events

#endregion
}