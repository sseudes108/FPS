using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public static Action<Bullet> OnBulletImpact;
    private Movement _movement;
    private Gun _gun;
    private TrailRenderer _trail;
    [SerializeField] private int _damageValue;

    private void Awake() {
        _movement = GetComponent<Movement>();
        _trail  = GetComponent<TrailRenderer>();
    }

    public int GetDamageValue(){
        return _damageValue;
    }

    public void Init(Gun gun, Transform firePoint) {
        _gun = gun;
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        Vector3 direction = firePoint.forward;
        _movement.SetRigidbodyDirection(direction);
        _trail.enabled = true;
        StartCoroutine(ReleaseBulletRoutine());
    }
    
    private void OnTriggerEnter(Collider other) {
        OnBulletImpact?.Invoke(this);
        if(other.CompareTag("Enemy")){
            if(other.TryGetComponent(out Health health)){
                health.TakeDamage(_damageValue);
            }
        }
        DisableBullet();
    }

    private IEnumerator ReleaseBulletRoutine(){
        yield return new WaitForSeconds(5f);
        DisableBullet();
        yield return null;
    }

    private void DisableBullet(){
        _trail.enabled = false;
        _gun.ReleaseFromPool(this);
    }

}
