using System;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _firerate;
    [SerializeField] private float _coolDown;
    [SerializeField] private float _bulletBurst;
    public float Firerate => _firerate;
    public float CoolDown => _coolDown;
    public float BulletBurst => _bulletBurst;

    private ObjectPool<Bullet> _bulletPool;

    private Character _character;

    private void Awake() {
        CreateBulletPool();
        _character = GetComponent<Character>();
    }

    public Transform GetFirePoint(){
        return _firePoint;
    }

    public void Shoot(){
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _character, _firePoint);
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