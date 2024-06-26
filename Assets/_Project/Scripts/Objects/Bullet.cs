using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public static Action<Bullet, Material> OnBulletImpact;
    private Movement _movement;
    private Gun _gun;
    private TrailRenderer _trail;
    [SerializeField] private int _damageValue;
    [SerializeField] private Material _playerMaterial, _enemyMaterial;
    private Material _currentMaterial;

    private void Awake() {
        _movement = GetComponent<Movement>();
        _trail  = GetComponent<TrailRenderer>();
    }

    public int GetDamageValue(){
        return _damageValue;
    }

    public void Init(Gun gun, Transform firePoint) {
        _gun = gun;
        SetMaterial();
        SetDirection(firePoint);
        _trail.enabled = true;
        StartCoroutine(ReleaseBulletRoutine());
    }
    
    private void OnTriggerEnter(Collider other) {
        OnBulletImpact?.Invoke(this, _currentMaterial);
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

    private void SetMaterial(){
        if(_gun.GetComponent<Character>() is Player){
            GetComponent<Renderer>().material = _playerMaterial;
            GetComponent<TrailRenderer>().material = _playerMaterial;
            _currentMaterial = _playerMaterial;
        }else{
            GetComponent<Renderer>().material = _enemyMaterial;
            GetComponent<TrailRenderer>().material = _enemyMaterial;
            _currentMaterial = _enemyMaterial;
        }     
    }

    private void SetDirection(Transform firePoint){
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        Vector3 direction = firePoint.forward;
        _movement.SetRigidbodyDirection(direction);
    }
}
