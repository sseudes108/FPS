using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public static Action<Bullet> OnBulletImpact;
    private Movement _movement;
    private Gun _gun;

    private void Awake() {
        _movement = GetComponent<Movement>();
    }

    public void Init(Gun gun, Transform firePoint) {
        _gun = gun;
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        Vector3 direction = firePoint.forward;
        _movement.SetRigidbodyDirection(direction);
        StartCoroutine(ReleaseBulletRoutine());
    }
    
    private void OnTriggerEnter() {
        OnBulletImpact?.Invoke(this);
        _gun.ReleaseFromPool(this);
    }

    private IEnumerator ReleaseBulletRoutine(){
        yield return new WaitForSeconds(5f);
        _gun.ReleaseFromPool(this);
        yield return null;
    }

}
