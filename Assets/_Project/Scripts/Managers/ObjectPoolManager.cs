using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour {
    [SerializeField] private Bullet _bulletPrefab;
    private ObjectPool<Bullet> _bulletPool;
    public ObjectPool<Bullet> BulletPool => _bulletPool;

    private void Awake() {
        CreateBulletPool();
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
}