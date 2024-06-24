using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour{

    [SerializeField] private Transform _firePoint;
    [SerializeField] private Bullet _bulletPrefab;

    private ObjectPool<Bullet> _bulletPool;

    private void Awake() {
        CreateBulletPool();
    }

    public void Shoot(){
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, _firePoint);
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