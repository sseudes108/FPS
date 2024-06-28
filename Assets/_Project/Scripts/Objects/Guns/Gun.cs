using UnityEngine;
using UnityEngine.Pool;

public abstract class Gun : MonoBehaviour{
    [SerializeField] protected float _recoilForce;
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Bullet _bulletPrefab;
    [SerializeField] protected float _firerate;
    [SerializeField] protected float _coolDown;
    [SerializeField] protected float _bulletBurst;
    public float Firerate => _firerate;
    public float CoolDown => _coolDown;
    public float BulletBurst => _bulletBurst;
    public float RecoilForce => _recoilForce;

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