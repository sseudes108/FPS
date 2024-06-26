using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Bullet : MonoBehaviour{
    public static Action<Bullet, Material> OnBulletImpact;
    private Movement _movement;
    private Gun _gun;
    private TrailRenderer _trail;
    private Character _character;
    [SerializeField] private int _damageValue;
    [SerializeField] private Material _playerMaterial, _enemyMaterial;
    private Material _currentMaterial;

    private void Awake() {
        _movement = GetComponent<Movement>();
        _trail  = GetComponent<TrailRenderer>();
    }

    public void Init(Gun gun, Transform firePoint) {
        SetGunAndCharacter(gun);
        SetMaterial();
        SetDirection(firePoint);
        _trail.enabled = true;
        StartCoroutine(ReleaseBulletRoutine());
    }

    private void OnTriggerEnter(Collider other) {
        if(_character is Player && other.CompareTag("Enemy")){
            HandleImpact(other);
            return;
        }else if (_character is Enemy && other.CompareTag("Player")){
            HandleImpact(other);
            return;
        }else{
            HandleImpact();
            return;
        }
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

    //No health on the other object
    private void HandleImpact(){
        OnBulletImpact?.Invoke(this, _currentMaterial);
        DisableBullet();
    }

    //Other is NPC
    private void HandleImpact(Collider other){
        OnBulletImpact?.Invoke(this, _currentMaterial);
        other.TryGetComponent(out Health health);
        health.TakeDamage(_damageValue);
        DisableBullet();
    }

    private void SetGunAndCharacter(Gun gun){
        _gun = gun;
        _character = _gun.GetComponent<Character>();
    }

    private void SetMaterial(){
        if(_character is Player){
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
