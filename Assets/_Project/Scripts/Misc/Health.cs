using Unity.Cinemachine;
using UnityEngine;

public class Health : MonoBehaviour{
    [field:SerializeField] public HealthManagerSO HealthManager { get; private set; }

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    private Character _character;

    private CinemachineImpulseSource _cinemachineImpulse;
    private bool _isDead = false;

    private void Awake() {
        _character = GetComponent<Character>();
        _cinemachineImpulse = GetComponent<CinemachineImpulseSource>();
    }

    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void HealDamage(int value){
        _currentHealth += value;
        if(_currentHealth > _maxHealth){
            _currentHealth = _maxHealth;
        }
        HealthManager.HealthChange(_currentHealth);
    }

    public void TakeDamage(int value){
        if(_isDead) { return; }
        _currentHealth -= value;

        if(_character is Player){
            HealthManager.HealthChange(_currentHealth);
            HealthManager.PlayerDamaged();
            _cinemachineImpulse.GenerateImpulseWithVelocity(new Vector3(0.3f, 0.1f, 0.3f));
        }
        
        if( _currentHealth <= 0 ){
            if(_character is Player){
                HealthManager.PlayerDied();
                _isDead = true;
            }else{
                Die();
            }
        }
    }

    public void Die(){
        Destroy(gameObject);
    }
}
