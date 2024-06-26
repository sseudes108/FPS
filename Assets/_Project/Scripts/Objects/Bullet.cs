using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public static Action<Bullet, Material> OnBulletImpact;

    [SerializeField] private int _damageValue;
    [SerializeField] private Material _playerMaterial, _enemyMaterial;
    private Material _currentMaterial;
    private TrailRenderer _trailRenderer;
    private Renderer _renderer;

    private Movement _movement;
    private Gun _gun;
    private Character _character;

    private void Awake() {
        _movement = GetComponent<Movement>();
        _renderer = GetComponent<Renderer>();
        _trailRenderer  = GetComponent<TrailRenderer>();
    }

    public void Init(Gun gun, Character character, Transform firePoint) {
        SetGunAndCharacter(gun, character);
        SetMaterial();
        SetDirection(firePoint);
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
        //Time to release bullet case dont hit anything
        yield return new WaitForSeconds(5f);
        DisableBullet();
        yield return null;
    }

    private void DisableBullet(){
        _trailRenderer.enabled = false;
        _gun.ReleaseFromPool(this);
    }

    //No health on the other object
    private void HandleImpact(){
        OnBulletImpact?.Invoke(this, _currentMaterial);
        DisableBullet();
    }

    //Other has health
    private void HandleImpact(Collider other){
        OnBulletImpact?.Invoke(this, _currentMaterial);
        other.TryGetComponent(out Health health);
        health.TakeDamage(_damageValue);
        DisableBullet();
    }

    private void SetGunAndCharacter(Gun gun, Character character){
        _gun = gun;
        _character = character;
    }

    private void SetMaterial(){
        if(_character is Player){
            _currentMaterial = _playerMaterial;
        }else{
            _currentMaterial = _enemyMaterial;
        }
        _renderer.material = _currentMaterial;
        _trailRenderer.material  = _currentMaterial;
    }

    private void SetDirection(Transform firePoint){
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        Vector3 direction = firePoint.forward;
        _movement.SetRigidbodyDirection(direction);
        _trailRenderer.enabled = true;
    }
}
