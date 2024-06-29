using UnityEngine;
using UnityEngine.Pool;

public abstract class Gun : MonoBehaviour{

    public enum WeaponTypes{
        Pistol = 0,
        Rifle = 1,
        MachineGun = 2,
        RocketLauncher = 3
    }

    [SerializeField] protected float _zoomAmount;
    [SerializeField] protected WeaponTypes _weaponType;
    [SerializeField] protected float _recoilForce;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected float _firerate;
    [SerializeField] private bool _canAutoFire;
    [SerializeField] private int _magazine;
    [SerializeField] private SoundSO _shootSound;

    public float ZoomAmount => _zoomAmount;
    public float Firerate => _firerate;
    public float RecoilForce => _recoilForce;
    public bool CanAutoFire => _canAutoFire;
    public int Magazine => _magazine;
    public SoundSO ShootSound => _shootSound;
    public WeaponTypes WeaponType => _weaponType;

    [SerializeField] protected Material _bulletMaterial;
    [SerializeField] protected int _damageValue;

    protected ObjectPool<Bullet> _bulletPool;
    protected Character _character;


    private void Awake() {
        CreateBulletPool();
        _character = GetComponent<Character>();
    }

    public Transform GetFirePoint(){
        return _firePoint;
    }

    public void Shoot(){
        var newBullet = _bulletPool.Get();
        newBullet.Init(this, _bulletMaterial, _damageValue, _character, _firePoint);
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
}