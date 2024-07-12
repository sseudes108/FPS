using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "GunEventHandlerSO", menuName = "FPS/EventHandlers/Gun", order = 1)]
public class GunEventHandlerSO : ScriptableObject {

    public ObjectPoolSO ObjectPool;
    public Bullet BulletPrefab;
    public ObjectPool<Bullet> BulletPool;
    public Vector3 FirePosition;

    public UnityEvent<Gun> OnWeaponPickUp;
    public UnityEvent<Gun> OnPlayerClosePickUp;
    public UnityEvent<bool> OnPlayerMoveOutRange;

    public UnityEvent<int, int, int> OnAmmoCountChange;
    public UnityEvent<PlayerGun, int> OnWeaponChange;
    
    public void OnEnable() {
        BulletPool ??= ObjectPool.CreatePool(BulletPrefab, FirePosition);

        OnWeaponPickUp ??= new UnityEvent<Gun>();
        OnPlayerClosePickUp ??= new UnityEvent<Gun>();
        OnPlayerMoveOutRange ??= new UnityEvent<bool>();

        OnAmmoCountChange ??= new UnityEvent<int, int, int>();
        OnWeaponChange ??= new UnityEvent<PlayerGun, int>();
    }

    public void WeaponPickedUp(Gun pickedGun) { OnWeaponPickUp?.Invoke(pickedGun); }
    public void PlayerClosePickUp(Gun gun) { OnPlayerClosePickUp?.Invoke(gun); }
    public void PlayerOutOfRangePickUp(bool isOutofRange) { OnPlayerMoveOutRange?.Invoke(isOutofRange); }

    public void ReleaseBulletFromPool(Bullet bullet){
        BulletPool.Release(bullet);
    }

    public void SetFirePosition(Vector3 firePoint){
        FirePosition = firePoint;
        ObjectPool.SetPosition(FirePosition);
    }
}