using System;
using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public static Action<Bullet, Material> OnBulletImpact;
    
    private int _damageValue;
    private Material _bulletMaterial;
    private TrailRenderer _trailRenderer;
    private Renderer _renderer;
    private BulletMovement _movement;
    private Gun _gun;
    private Character _character;

    private void Awake() {
        _movement = GetComponent<BulletMovement>();
        _renderer = GetComponent<Renderer>();
        _trailRenderer  = GetComponent<TrailRenderer>();
    }

    public void Init(Gun gun, Material material, int damageValue, Character character, Transform firePoint) {
        SetGunAndCharacter(gun, character);
        SetMaterial(material);
        SetDirection(firePoint);
        _damageValue = damageValue;
        StartCoroutine(ReleaseBulletRoutine());
    }

    private void OnTriggerEnter(Collider other) {
        var objectTag = other.tag;

        switch (objectTag){
            case "Enemy":
                if(_character is Player){
                    HandleImpact(other);
                }
            break;
            
            case "Player":
                if(_character is Enemy){
                    HandleImpact(other);
                }
            break;

            case "NoHit":
            break;

            default:
                HandleImpact(other);
            break;
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

    private void HandleImpact(Collider other){
        // OnBulletImpact?.Invoke(this, _bulletMaterial);
        if(other.TryGetComponent(out Health health)){
            health.TakeDamage(CalculateDamage());
        }
        DisableBullet();
    }

    private int CalculateDamage(){
        if(transform.position.y > 1.22){
            _damageValue *= 30;
        }
        return _damageValue;
    }

    private void SetGunAndCharacter(Gun gun, Character character){
        _gun = gun;
        _character = character;
    }

    private void SetMaterial(Material material){
        _bulletMaterial = material;
        _renderer.material = _bulletMaterial;
        _trailRenderer.material  = _bulletMaterial;
    }

    private void SetDirection(Transform firePoint){
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        Vector3 direction = firePoint.forward;
        _movement.SetDirection(direction);
        _trailRenderer.enabled = true;
    }
}
