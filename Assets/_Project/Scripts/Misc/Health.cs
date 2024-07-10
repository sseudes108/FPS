using System;
using Unity.Cinemachine;
using UnityEngine;

public class Health : MonoBehaviour{
    public static Action<int> OnHealthChange;
    public static Action OnPlayerDamaged;
    public static Action OnPlayerDied;

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
        OnHealthChange?.Invoke(_currentHealth);
    }

    public void TakeDamage(int value){
        if(_isDead) { return; }
        _currentHealth -= value;

        if(_character is Player){
            OnHealthChange?.Invoke(_currentHealth);
            OnPlayerDamaged?.Invoke();
            _cinemachineImpulse.GenerateImpulseWithVelocity(new Vector3(0.3f, 0.1f, 0.3f));
        }
        
        if( _currentHealth <= 0 ){
            if(_character is Player){
                OnPlayerDied?.Invoke();
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
