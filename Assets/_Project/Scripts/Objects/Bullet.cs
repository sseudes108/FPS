using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public static Action<Bullet> OnBulletImpact;
    private Movement _movement;
    private Gun _gun;
    private TrailRenderer _trail;

    private void Awake() {
        _movement = GetComponent<Movement>();
        _trail  = GetComponent<TrailRenderer>();
    }

    public void Init(Gun gun, Transform firePoint) {
        _gun = gun;
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        Vector3 direction = firePoint.forward;
        _movement.SetRigidbodyDirection(direction);
        _trail.enabled = true;
        StartCoroutine(ReleaseBulletRoutine());
    }
    
    private void OnTriggerEnter() {
        OnBulletImpact?.Invoke(this);
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
