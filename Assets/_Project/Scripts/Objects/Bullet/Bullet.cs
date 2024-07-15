using System.Collections;
using UnityEngine;

// TimeStamp in Unity Project = 0.001
public class Bullet : MonoBehaviour{
    [SerializeField] private VisualManagerSO VisualsManager;
    [SerializeField] private GunManagerSO GunManager;
    [SerializeField] private PlayerStateMachineSO _playerManager;

    private int _damageValue;
    private Material _bulletMaterial;
    private TrailRenderer _trailRenderer;
    private Renderer _renderer;

    [SerializeField] private float _moveSpeed;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        _trailRenderer  = GetComponent<TrailRenderer>();
    }

    private void FixedUpdate() {
        Vector3 movement = _moveSpeed * Time.deltaTime * _direction;
        _rigidbody.MovePosition(_rigidbody.position + movement);
    }

    public void Init(Material material, int damageValue, Transform firePoint){
        SetDirectionAndPosition(firePoint);
        SetMaterial(material);
        _damageValue = damageValue;

        if(gameObject.activeSelf){
            StartCoroutine(ReleaseBulletRoutine());
        }
    }

    private void OnTriggerEnter(Collider other) {
        
        var objectTag = other.tag;
        
        switch (objectTag){
            // case "Enemy":
            //     HandleImpact(other);
            // break;
            
            // case "Player":
            //     HandleImpact(other);
            // break;

            case "NoHit":
                // Debug.Log("HIT");
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
        GunManager.ReleaseBulletFromPool(this);
    }

    private void HandleImpact(Collider other){
        var positionToImpactEffect = transform.position - transform.forward * 0.2f;
        VisualsManager.BulletImpactEffect(_playerManager.Player, positionToImpactEffect, _bulletMaterial);
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

    private void SetMaterial(Material material){
        _bulletMaterial = material;
        _renderer.material = _bulletMaterial;
        _trailRenderer.material  = _bulletMaterial;
    }

    private void SetDirectionAndPosition(Transform firePoint){
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        _direction = firePoint.forward;
        _trailRenderer.enabled = true;
    }
}
