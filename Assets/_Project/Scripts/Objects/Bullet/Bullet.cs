using System.Collections;
using UnityEngine;

// TimeStamp in Unity Project = 0.001
public class Bullet : MonoBehaviour{
    [SerializeField] private VisualsEventHandlerSO VisualsManager;
    [SerializeField] private GunEventHandlerSO GunManager;

    private int _damageValue;
    private Material _bulletMaterial;
    private TrailRenderer _trailRenderer;
    private Renderer _renderer;
    private Gun _gun;
    private Character _character;

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

    public void Init(Gun gun, Material material, int damageValue, Character character, Transform firePoint){
        SetDirectionAndPosition(firePoint);
        SetGunAndCharacter(gun, character);
        SetMaterial(material);
        _damageValue = damageValue;

        if(gameObject.activeSelf){
            StartCoroutine(ReleaseBulletRoutine());
        }
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
                Debug.Log("HIT");
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
        VisualsManager.BulletImpactEffect(this, _bulletMaterial);
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

    private void SetDirectionAndPosition(Transform firePoint){
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        _direction = firePoint.forward;
        _trailRenderer.enabled = true;
    }
}
