using System.Collections;
using UnityEngine;

// TimeStamp in Unity Project = 0.001
public class Bullet : MonoBehaviour{
    [SerializeField] private VisualManagerSO VisualsManager;
    [SerializeField] private GunManagerSO GunManager;
    [SerializeField] private PlayerStateMachineSO _playerManager;

    private int _damageValue;
    private Material _bulletMaterial;
    public TrailRenderer TrailRenderer { get; private set; }
    public Renderer _renderer;

    [SerializeField] private float _moveSpeed;
    private Rigidbody _rigidbody;
    private Vector3 _direction;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        _renderer = GetComponent<Renderer>();
        TrailRenderer  = GetComponent<TrailRenderer>();
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

    public virtual void OnCollisionEnter(Collision other) {
        HandleImpact(other);
    }

    public IEnumerator ReleaseBulletRoutine(){
        //Time to release bullet case dont hit anything
        yield return new WaitForSeconds(5f);
        DisableBullet();
        yield return null;
    }

    private void DisableBullet(){
        TrailRenderer.enabled = false;
        GunManager.ReleaseBulletFromPool(this);
    }

    private void HandleImpact(Collision other){
        ContactPoint contact = other.contacts[0];
        contact.otherCollider.TryGetComponent<Health>(out Health health);
        if(health != null){
            health.TakeDamage(_damageValue);
        }
        VisualsManager.BulletImpactEffect(_playerManager.Player, contact.point, _bulletMaterial);
    }

    public void SetDamage(int value){
        _damageValue = value;
    }

    public void SetMaterial(Material material){
        _bulletMaterial = material;
        _renderer.material = _bulletMaterial;
        TrailRenderer.material  = _bulletMaterial;
    }

    public void SetDirectionAndPosition(Transform firePoint){
        transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
        _direction = firePoint.forward;
        TrailRenderer.enabled = true;
    }

    public void SetDirection(Transform firePoint){
        _direction = firePoint.forward;
    }
}
