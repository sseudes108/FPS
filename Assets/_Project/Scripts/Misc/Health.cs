using UnityEngine;

public class Health : MonoBehaviour{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;

    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int value){
        _currentHealth -= value;
        Debug.Log(_currentHealth);
        if( _currentHealth <= 0 ){
            Die();
        }
    }

    public void HealDamage(int value){
        _currentHealth += value;
    }

    public void Die(){
        Destroy(gameObject);
    }
}
