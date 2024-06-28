using System;
using UnityEngine;

public class Health : MonoBehaviour{
    public static Action<int> OnDamageTaken;
    public static Action OnPlayerDied;

    [SerializeField] private int _maxHealth;
    [SerializeField] private int _currentHealth;
    private Character _character;

    private void Awake() {
        _character = GetComponent<Character>();
    }

    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void HealDamage(int value){
        _currentHealth += value;
    }

    public void TakeDamage(int value){
        _currentHealth -= value;

        if(_character is Player){
            OnDamageTaken?.Invoke(value);
        }
        
        if( _currentHealth <= 0 ){
            if(_character is Player){
                OnPlayerDied?.Invoke();
            }else{
                Die();
            }
        }
    }

    public void Die(){
        Destroy(gameObject);
    }
}
