using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "GunManagerSO", menuName = "FPS/Managers/Gun", order = 1)]
public class GunManagerSO : ScriptableObject {
    [SerializeField] private ObjectPoolManagerSO _objectPool;
    [SerializeField] private Bullet _bulletPrefab;
    public ObjectPool<Bullet> BulletPool { get; private set; }
    public Vector3 FirePosition { get; private set; }

    [HideInInspector] public UnityEvent<Gun> OnWeaponPickUp;
    [HideInInspector] public UnityEvent<Gun> OnPlayerClosePickUp;
    [HideInInspector] public UnityEvent<bool> OnPlayerMoveOutRange;

    [HideInInspector] public UnityEvent<int, int, int> OnAmmoCountChange;
    [HideInInspector] public UnityEvent<PlayerGun, int> OnWeaponChange;
    
    public void OnEnable() {
        BulletPool ??= _objectPool.CreatePool(_bulletPrefab, FirePosition);

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
        _objectPool.SetPosition(FirePosition);
    }
}