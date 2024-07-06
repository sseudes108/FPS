using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour {
    [SerializeField] private Bullet _bulletPrefab;
    public ObjectPool<Bullet> BulletPool {get; private set;}
    private Transform _firePoint;

    public ObjectPool<AudioSource> AudioPool {get; private set;}
    public AudioSource _audioPrefab;

    private void OnEnable() {
        Gun.OnShoot += Gun_OnShoot;
    }

    private void OnDisable() {
        Gun.OnShoot += Gun_OnShoot;
    }

    private void Gun_OnShoot(Transform firePoint){
        _firePoint = firePoint;
    }

    private void Awake() {
        CreateBulletPool();
        CreateAudioSourcePool();
    }

    private void CreateBulletPool(){
        BulletPool = new ObjectPool<Bullet>(()=>{
            return Instantiate(_bulletPrefab, _firePoint.position, Quaternion.identity);
        }, newBullet =>{
            newBullet.transform.position = _firePoint.position;
            newBullet.gameObject.SetActive(true);
        }, newBullet =>{
            newBullet.gameObject.SetActive(false);
        }, newBullet =>{
            Destroy(newBullet);
        }, false, 50, 70);
    }

    public void CreateAudioSourcePool(){
        AudioPool = new ObjectPool<AudioSource>(()=>{
            return Instantiate(_audioPrefab);
        }, newAudio =>{
            newAudio.gameObject.SetActive(true);
        }, newAudio =>{
            newAudio.gameObject.SetActive(false);
        }, newAudio =>{
            Destroy(newAudio);
        }, false, 50, 70);
    }
}