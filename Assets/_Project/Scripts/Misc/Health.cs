using UnityEngine;

public class Health : MonoBehaviour{
    [SerializeField] private int _maxHealth;
    private int _currentHealth;
    private Character _character;

    private void Awake() {
        _character = GetComponent<Character>();
    }

    private void Start() {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int value){
        _currentHealth -= value;
        if( _currentHealth <= 0 ){
            if(_character is Player){
                // _character.gameObject.SetActive(false);
                // Time.timeScale = 0;
            }else{
                Die();
            }
        }
    }

    public void HealDamage(int value){
        _currentHealth += value;
    }

    public void Die(){
        Destroy(gameObject);
    }
}
