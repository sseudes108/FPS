using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "GunEventHandlerSO", menuName = "FPS/EventHandlers/Gun", order = 1)]
public class GunEventHandlerSO : ScriptableObject {
    public Bullet BulletPrefab;
    public ObjectPool<Bullet> BulletPool;
    private Transform FirePoint;

    public UnityEvent<Gun> OnWeaponPickUp;
    public UnityEvent<Gun> OnPlayerClosePickUp;
    public UnityEvent<bool> OnPlayerMoveOutRange;

    public UnityEvent<int, int, int> OnAmmoCountChange;
    public UnityEvent<PlayerGun, int> OnWeaponChange;

    private void OnEnable() {
        BulletPool ??= CreateBulletPool();

        OnWeaponPickUp ??= new UnityEvent<Gun>();
        OnPlayerClosePickUp ??= new UnityEvent<Gun>();
        OnPlayerMoveOutRange ??= new UnityEvent<bool>();

        OnAmmoCountChange ??= new UnityEvent<int, int, int>();
        OnWeaponChange ??= new UnityEvent<PlayerGun, int>();
    }

    public void SetFirePoint(Transform firePoint){
        FirePoint = firePoint;
    }

    public ObjectPool<Bullet> CreateBulletPool(){
        var bulletPool = new ObjectPool<Bullet>(()=>{
            return Instantiate(BulletPrefab, FirePoint.position, Quaternion.identity);
        }, newBullet =>{
            newBullet.transform.position = FirePoint.position;
            newBullet.gameObject.SetActive(true);
        }, newBullet =>{
            newBullet.gameObject.SetActive(false);
        }, newBullet =>{
            Destroy(newBullet);
        }, false, 50, 70);

        return bulletPool;
    }

    public void WeaponPickedUp(Gun pickedGun){
        OnWeaponPickUp?.Invoke(pickedGun);
    }

    public void PlayerClosePickUp(Gun gun){
        OnPlayerClosePickUp?.Invoke(gun);
    }

    public void PlayerOutOfRangePickUp(bool isOutofRange){
        OnPlayerMoveOutRange?.Invoke(isOutofRange);
    }
}